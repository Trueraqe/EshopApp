using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EshopCore.Utils
{
    public class RegisterUserValidator : AbstractValidator<User>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(3, 20)                  //"Nazwa użytkownika musi mieć od 3 do 20 znaków."
                .Matches("^[a-zA-Z0-9]+$");     //"Nazwa użytkownika może składać się wyłącznie z liter i cyfr (bez spacji i znaków specjalnych)."
                                                //.Matches("^[a-zA-Z0-9_]+$");    //"Nazwa użytkownika może zawierać tylko litery (bez polskich znaków), cyfry oraz podkreślnik."

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()                     //"Hasło nie może być puste."
                .MinimumLength(8)               //"Hasło musi mieć min. 8 znaków."
                .Matches("[A-Z]")               //"Hasło musi zawierać wielką literę."
                .Matches("[a-z]")               //"Hasło musi zawierać małą literę."
                .Matches("[0-9]")               //"Hasło musi zawierać cyfrę."
                .Matches("[^a-zA-Z0-9]");       //"Hasło musi zawierać znak specjalny."
        }
    }
    public class Validators : IValidators
    {
        private static readonly PasswordHasher<User> _passwordHasher = new();
        public static bool VerifyUserPassword(User user, string inputPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, inputPassword);

            return result == PasswordVerificationResult.Success;
        }
        public static string ClearDateTime()
        {
            DateTime now = DateTime.Now;
            return now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string SQL_DB_Path(string fileName)
        {
            return @"C:\Users\teflo\source\repos\EshopApp\EshopCore\Database SQLite\" + fileName;
        }
        public static void WriteFilePath(string fileName, string content)
        {
            // 1. Ścieżka do katalogu projektu (3 poziomy w górę względem bin/Debug/netX)
            string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");

            // 2. Folder projektu + podfolder Database
            string databaseFolder = Path.Combine(projectPath, "Database");

            // 3. Tworzymy folder jeśli go nie ma
            Directory.CreateDirectory(databaseFolder);

            // 4. Pełna ścieżka do pliku
            string filePath = Path.GetFullPath(Path.Combine(databaseFolder, fileName));

            // 5. Zapisujemy zawartość
            File.WriteAllText(filePath, content);

            // 6. (Opcjonalnie) wypisz ścieżkę do konsoli
            //Console.WriteLine($"Plik zapisany: {filePath}");
        }
        public static void LoadFromJson()
        {
            string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
            string databaseFolder = Path.Combine(projectPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            string usersfilePath = Path.GetFullPath(Path.Combine(databaseFolder, "UsersDB.json"));
            string usersjsonFromFile = File.ReadAllText(usersfilePath);
            var usersFromFile = JsonSerializer.Deserialize<List<User>>(usersjsonFromFile) ?? new List<User>();
            FakeDatabase.Users = usersFromFile;

            string productsfilePath = Path.GetFullPath(Path.Combine(databaseFolder, "ProductsDB.json"));
            string productsjsonFromFile = File.ReadAllText(productsfilePath);
            var productsFromFile = JsonSerializer.Deserialize<List<Product>>(productsjsonFromFile) ?? new List<Product>();
            FakeDatabase.Products = productsFromFile;

            string ordersfilePath = Path.GetFullPath(Path.Combine(databaseFolder, "OrdersDB.json"));
            string ordersjsonFromFile = File.ReadAllText(ordersfilePath);
            var ordersFromFile = JsonSerializer.Deserialize<List<Order>>(ordersjsonFromFile) ?? new List<Order>();
            FakeDatabase.Orders = ordersFromFile;
        }
        public static void SaveToJson()
        {
            string jsonUsers = JsonSerializer.Serialize(FakeDatabase.Users, new JsonSerializerOptions { WriteIndented = true });
            string jsonProducts = JsonSerializer.Serialize(FakeDatabase.Products, new JsonSerializerOptions { WriteIndented = true });
            string jsonOrders = JsonSerializer.Serialize(FakeDatabase.Orders, new JsonSerializerOptions { WriteIndented = true });

            WriteFilePath("UsersDB.json", jsonUsers);
            WriteFilePath("ProductsDB.json", jsonProducts);
            WriteFilePath("OrdersDB.json", jsonOrders);
        }

        public static void ChangeConsole(ConsoleColor color, string input)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        public static int WriteReadInt(string write)
        {
            while (true)
            {
                Console.Write(write);
                var input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                {
                    return value;
                }
                ChangeConsole(ConsoleColor.Red, "Musisz podać cyfrę.");
                continue;
            }
        }
        public static decimal WriteReadDeci(string write)
        {
            while (true)
            {
                Console.Write(write);
                var input = Console.ReadLine();
                if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
                {
                    return value;
                }
                ChangeConsole(ConsoleColor.Red, "Musisz podać cyfrę.");
                continue;
            }
        }
        public static string WriteReadStr(string write)
        {
            while (true)
            {
                Console.Write(write);
                var input = Console.ReadLine() ?? string.Empty;
                if (input.All(char.IsLetter))
                {
                    return input;
                }
                ChangeConsole(ConsoleColor.Red, "Musisz podać nazwę.");
                continue;
            }
        }
        public static string WriteReadBoth(string write)
        {
            while (true)
            {
                Console.Write(write);
                var input = Console.ReadLine() ?? string.Empty;
                if (input.Any(char.IsLetterOrDigit)) 
                {
                    return input;
                }
                ChangeConsole(ConsoleColor.Red, "To pole nie może pozostać puste.");
                continue;
            }
        }
        public static bool Payment()
        {
            ChangeConsole(ConsoleColor.DarkMagenta, "Wybierz metodę płatności:\n1. Karta\n2. Blik\n3. Gotówka\n10. Cofnij");

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            var nrKarty = WriteReadBoth("Podaj 16-cyfrowy numer karty: ");
                            if (nrKarty.Length == 16 && nrKarty.All(char.IsDigit))
                            {
                                ChangeConsole(ConsoleColor.Green, "Płatność zakończona sukcesem.");
                                return true;
                            }
                            else
                            {
                                ChangeConsole(ConsoleColor.Red, "Numer karty jest nieprawidłowy spróbuj ponownie.");
                                ChangeConsole(ConsoleColor.DarkMagenta, "Wybierz metodę płatności:\n1. Karta\n2. Blik\n3. Gotówka\n10. Cofnij");
                                break;
                            }
                        }
                        break;

                    case "2":
                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            var nrKarty = WriteReadBoth("Podaj 6-cyfrowy numer blik: ");
                            if (nrKarty.Length == 6 && nrKarty.All(char.IsDigit))
                            {
                                ChangeConsole(ConsoleColor.Green, "Płatność zakończona sukcesem.");
                                return true;
                            }
                            else
                            {
                                ChangeConsole(ConsoleColor.Red, "Numer blik jest nieprawidłowy spróbuj ponownie.");
                                ChangeConsole(ConsoleColor.DarkMagenta, "Wybierz metodę płatności:\n1. Karta\n2. Blik\n3. Gotówka\n10. Cofnij");
                                break;
                            }
                        }
                        break;

                    case "3":
                        ChangeConsole(ConsoleColor.Green, "Płatność zakończona sukcesem.");
                        return true;

                    case "10":
                        ChangeConsole(ConsoleColor.Red, "Płatność została przerwana.");
                        return false;

                    default:
                        ChangeConsole(ConsoleColor.Red, "Błąd, nieprawidłowa opcja.");
                        ChangeConsole(ConsoleColor.DarkMagenta, "Wybierz metodę płatności:\n1. Karta\n2. Blik\n3. Gotówka\n10. Cofnij");
                        break;
                }
            }
        }
        public static async Task OrderStatusChange(Order order)
        {
            ShopContext _context = new();

            if (order != null)
            {
                ChangeConsole(ConsoleColor.DarkMagenta, "Wybierz nowy status:\n1.New\n2.Shipped\n3.Cancelled\n4.Delivered");

                switch (Console.ReadLine())
                {
                    case "1":
                        order.New();

                        ChangeConsole(ConsoleColor.Green, "Status zamówienia \"New\" został ustawiony pomyślnie.");
                        break;

                    case "2":
                        order.Shipped();

                        ChangeConsole(ConsoleColor.Green, "Status zamówienia \"Shipped\" został ustawiony pomyślnie.");
                        break;

                    case "3":
                        order.Cancelled();

                        ChangeConsole(ConsoleColor.Green, "Status zamówienia \"Cancelled\" został ustawiony pomyślnie.");
                        break;

                    case "4":
                        order.Delivered();

                        ChangeConsole(ConsoleColor.Green, "Status zamówienia \"Delivered\" został ustawiony pomyślnie.");
                        break;

                    default:
                        ChangeConsole(ConsoleColor.Red, "Wybrany status nieistnieje.");
                        break;
                }
                return;
            }
            ChangeConsole(ConsoleColor.Red, "Zamówienie o podanym ID nieistnieje.");
        }
    }
}
