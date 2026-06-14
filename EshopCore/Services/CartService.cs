using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.Services
{
    public class CartService : ICartService
    {
        private readonly ShopContext _context;
        public CartService(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProductToCart(int userId, int productId, int quantity)
        {
            var cart_product = _context.Cart.FirstOrDefault(p => p.UserId == userId && p.ProductId == productId);

            var sql_product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == productId);

            if (sql_product == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Produkt o podanym ID nie istnieje.");
                return false;
            }
            int quantityInCart = _context.Cart.Where(i => i.ProductId == productId && i.UserId == userId).Sum(i => i.Quantity);

            if (sql_product.Stock < (quantityInCart + quantity))
            {
                Validators.ChangeConsole(ConsoleColor.Red, $"Niewystarczająca ilość produktu. W magazynie: {sql_product.Stock}, w koszyku: {quantityInCart}, próbujesz dodać: {quantity}");
                return false;
            }
            if (cart_product != null)
            {
                cart_product.Quantity += quantity;

                Validators.ChangeConsole(ConsoleColor.Yellow, $"Zmieniono stan produktu na {cart_product.Quantity}");

                _context.Update(cart_product);

                await _context.SaveChangesAsync();
                return true;
            }
            var newItem = new CartItem(userId, productId, quantity);

            await _context.AddAsync(newItem);

            await _context.SaveChangesAsync();

            Validators.ChangeConsole(ConsoleColor.Green, "Produkt dodany pomyślnie.");

            sql_product.Info();
            return true;
        }

        public async Task<bool> RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var cart_product = _context.Cart.FirstOrDefault(p => p.UserId == userId && p.ProductId == productId);

            var sql_product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == productId);

            if (sql_product == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Nie ma takiego produktu w bazie.");
                return false;
            }
            if (cart_product == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Nie ma takiego produktu w koszyku.");
                return false;
            }
            if (cart_product.Quantity == quantity)
            {
                Validators.ChangeConsole(ConsoleColor.Green, "Produkt usunięty pomyślnie.");

                sql_product.Info();

                _context.Remove(cart_product);

                await _context.SaveChangesAsync();
                return true;
            }
            else if (cart_product.Quantity < quantity)
            {
                Validators.ChangeConsole(ConsoleColor.Red, $"Ilość produktu w koszyku jest mniejsza niż wpisana (Łącznie w koszyku: {cart_product.Quantity})");
                return false;
            }
            cart_product.Quantity -= quantity;

            Validators.ChangeConsole(ConsoleColor.Yellow, $"Zmieniono stan produktu na {cart_product.Quantity}");

            _context.Update(cart_product);

            await _context.SaveChangesAsync();
            return true;
        }

        public Task ClearUserCart(int userId)
        {
            _context.Cart.Where(u => u.UserId == userId).ExecuteDelete();

            Validators.ChangeConsole(ConsoleColor.Green, "Twój koszyk został wyczyszczony.");
            return Task.CompletedTask;
        }

        public async Task<decimal> GetTotalCartPrice(int userId)
        {
            Validators.ChangeConsole(ConsoleColor.Cyan, "Koszyk:\n----------------------------------------------------------------------------------------------------------------");

            decimal cost = 0;

            var cart = await _context.Cart.AsNoTracking().Where(i => i.UserId == userId).Include(p => p.Product).ToListAsync();

            foreach (var item in cart)
            {
                cost += item.Product.Price * item.Quantity;

                Validators.ChangeConsole(ConsoleColor.Yellow, $"ID: {item.Product.Id}, Nazwa: {item.Product.Name}, Kategoria: {item.Product.Category}, Cena: {item.Product.Price}zł/szt, Ilość: {item.Quantity}, Opis: {item.Product.Description}");
            }
            Validators.ChangeConsole(ConsoleColor.Yellow, $"----------------------------------------------------------------------------------------------------------------\nCena za całość: {cost} zł");
            return cost;
        }
    }
}
