using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Enums;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace EshopCore.Utils
{
    public class MenuRenderer
    {
        public void Start()
        {
            ShopContext _context = new();

            IValidator<User> _validator = new RegisterUserValidator();

            IUserService us = new UserService(_context, _validator);
            
            ICartService cs = new CartService(_context);

            IProductService ps = new ProductService(_context);

            IOrderService os = new OrderService(_context);

            IAuthService auth = new AuthService(_context);

            Validators.ChangeConsole(ConsoleColor.DarkGray, "EshopConsoleAPP (SQLite version) made by Truerage\n");

            var info1 = "1. Logowanie\n2. Rejestracja\n3. Wyjście\n";

            var info2 = "1. Przeglądanie produktów\n2. Wyszukiwanie po nazwie\n3. Filtrowanie po kategorii\n4. Dodawanie do koszyka\n5. Usuwanie produktu z koszyka\n6. Sprawdź koszyk\n7. Składanie zamówienia\n8. Przeglądanie historii zamówień\n9. Wyczyść koszyk\n10. Wyloguj\n";

            var info3 = "1. Dodawanie produtków\n2. Usuwanie produktów\n3. Edytowanie produktów\n4. Przeglądanie produktów\n5. Wyszukaj zamówień użytkownika\n6. Przeglądanie wszystkich zamówień\n7. Zmiana statusu zamówień\n8. Wyszukaj użytkownika\n9. Lista wszystkich użytkowników\n10. Wyloguj\n";

            var info4 = "Login może składać się wyłącznie z liter i cyfr (bez spacji i znaków specjalnych) i musi mieć od 3 do 20 znaków.\nHasło musi mieć min. 8 znaków, zawierać małą i wielką literę oraz cyfrę i znak specjalny.";

            while (true)
            {
                var text1 = Validators.WriteReadBoth(info1);
                if (text1 == "1")
                {
                    var current = auth.Login(Validators.WriteReadBoth("Username: "), Validators.WriteReadBoth("Password: "));
                    auth.CurrentUser();
                    if (current != null && current.Role == UserRole.Customer)
                    {
                        while (true)
                        {
                            var text2 = Validators.WriteReadBoth(info2);
                            if (text2 == "1")
                            {
                                ps.GetAllProducts();
                                continue;
                            }
                            else if (text2 == "2")
                            {
                                ps.SearchProductByName(Validators.WriteReadBoth("Podaj nazwę produktu: "));
                                continue;
                            }
                            else if (text2 == "3")
                            {
                                ps.FilterProductsByCategory(Validators.WriteReadBoth("Podaj kategorię produktu: "));
                                continue;
                            }
                            else if (text2 == "4")
                            {
                                cs.AddProductToCart(current.Id, Validators.WriteReadInt("Dodaj do koszyka ID: "), Validators.WriteReadInt("Ilość: "));
                                continue;
                            }
                            else if (text2 == "5")
                            {
                                cs.RemoveProductFromCart(current.Id, Validators.WriteReadInt("Usuń z koszyka ID: "), Validators.WriteReadInt("Podaj ilość: "));
                                continue;
                            }
                            else if (text2 == "6")
                            {
                                cs.GetTotalCartPrice(current.Id);
                                continue;
                            }
                            else if (text2 == "7")
                            {
                                os.CreateOrder(current.Id);
                                continue;
                            }
                            else if (text2 == "8")
                            {
                                os.LoggedInUserOdrersHistory(current.Id);
                                continue;
                            }
                            else if (text2 == "9")
                            {
                                cs.ClearUserCart(current.Id);
                                continue;
                            }
                            else if (text2 == "10")
                            {
                                auth.Logout();
                                break;
                            }
                            else
                            {
                                Validators.ChangeConsole(ConsoleColor.Red, "Wybrana opcja nie istnieje, spróbuj ponownie.");
                                continue;
                            }
                        }
                    }
                    else if (current != null && current.Role == UserRole.Admin)
                    {
                        while (true)
                        {
                            var text2 = Validators.WriteReadBoth(info3);
                            if (text2 == "1")
                            {
                                ps.AddProduct(Validators.WriteReadBoth("Nazwa: "), Validators.WriteReadBoth("Kategoria: "), Validators.WriteReadDeci("Cena: "), Validators.WriteReadInt("Stan: "), Validators.WriteReadBoth("Opis: "));
                                continue;
                            }
                            else if (text2 == "2")
                            {
                                ps.RemoveProduct(Validators.WriteReadInt("ID: "));
                                continue;
                            }
                            else if (text2 == "3")
                            {
                                ps.UpdateProduct(Validators.WriteReadInt("ID: "), Validators.WriteReadBoth("Nazwa: "), Validators.WriteReadBoth("Kategoria: "), Validators.WriteReadDeci("Cena: "), Validators.WriteReadInt("Stan: "), Validators.WriteReadBoth("Opis: "));
                            }
                            else if (text2 == "4")
                            {
                                ps.GetAllProducts();
                                continue;
                            }
                            else if (text2 == "5")
                            {
                                os.GetUserOrdersHistoryByUsername(Validators.WriteReadBoth("Nazwa użytkownika: "));
                                continue;
                            }
                            else if (text2 == "6")
                            {
                                os.GetAllUsersOrders();
                                continue;
                            }
                            else if (text2 == "7")
                            {
                                os.ChangeOrderStatus(Validators.WriteReadInt("ID zamówienia: "));
                                continue;
                            }
                            else if (text2 == "8")
                            {
                                us.GetUserByUsername(Validators.WriteReadBoth("Nazwa: "));
                                continue;
                            }
                            else if (text2 == "9")
                            {
                                us.GetAllUsers();
                                continue;
                            }
                            else if (text2 == "10")
                            {
                                auth.Logout();
                                break;
                            }
                            else
                            {
                                Validators.ChangeConsole(ConsoleColor.Red, "Wybrana opcja nie istnieje, spróbuj ponownie.");
                                continue;
                            }
                        }
                    }
                }
                else if (text1 == "2")
                {
                    Validators.ChangeConsole(ConsoleColor.DarkMagenta, info4);
                    us.RegisterUser(Validators.WriteReadBoth("Username: "), Validators.WriteReadBoth("Email: "), Validators.WriteReadBoth("Password: "));
                    continue;
                }
                else if (text1 == "3")
                {
                    Validators.ChangeConsole(ConsoleColor.DarkMagenta, "Koniec Programu.");
                    break;
                }
                else
                {
                    Validators.ChangeConsole(ConsoleColor.Red, "Wybrana opcja nie istnieje, spróbuj ponownie.");
                    continue;
                }
            }
        }
    }
}
