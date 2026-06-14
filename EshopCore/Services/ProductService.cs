using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EshopCore.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopContext _context;
        public ProductService(ShopContext context)
        {
            _context = context;
        }

        public async Task AddProduct(string name, string category, decimal price, int stock, string description)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category) || decimal.IsNegative(price) || int.IsNegative(stock) || string.IsNullOrWhiteSpace(description))
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Niepoprawne dane.");
                return;
            }
            var new_product = new Product(name, category, price, stock, description);

            Validators.ChangeConsole(ConsoleColor.Green, "Produkt dodany pomyślnie.");

            Validators.ChangeConsole(ConsoleColor.Yellow, $"Nazwa: {name}, Kategoria: {category}, Cena: {price}zł, Na stanie: {stock}, Opis: {description}");

            await _context.AddAsync(new_product);

            await _context.SaveChangesAsync();
            return;
        }

        public async Task UpdateProduct(int id, string name, string category, decimal price, int stock, string description)
        {
            var sql_product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (sql_product == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Produkt o podanym ID nie istnieje.");
                return;
            }
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category) || decimal.IsNegative(price) || int.IsNegative(stock) || string.IsNullOrWhiteSpace(description))
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Niepoprawne dane.");
                return;
            }
            sql_product.Name = name;
            sql_product.Category = category;
            sql_product.Price = price;
            sql_product.Stock = stock;
            sql_product.Description = description;

            Validators.ChangeConsole(ConsoleColor.Green, "Produkt zaktualizowany pomyślnie.");

            sql_product.Info();

            _context.Update(sql_product);

            await _context.SaveChangesAsync();
            return;
        }

        public async Task RemoveProduct(int id)
        {
            var sql_product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (sql_product == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Produkt o podanmy ID nie istnieje.");
                return;
            }
            Validators.ChangeConsole(ConsoleColor.Green, "Produkt usunięty pomyślnie.");

            sql_product.Info();

            _context.Remove(sql_product);

            await _context.SaveChangesAsync();
            return;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            await _context.Products.AsNoTracking().ForEachAsync(p => p.Info());
            return _context.Products.ToList();
        }

        public async Task<Product> SearchProductByName(string name)
        {
            var sql_product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Name.ToLower() == name.ToLower());

            if (sql_product == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Produkt nie istnieje.");
                return sql_product!;
            }
            sql_product.Info();
            return sql_product;
        }

        public async Task<List<Product>> FilterProductsByCategory(string category)
        {
            var sql_products_list = _context.Products.AsNoTracking().Where(p => p.Category.ToLower() == category.ToLower()).ToList();

            if (!sql_products_list.Any())
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Kategoria nie istnieje.");
                return sql_products_list!;
            }
            Validators.ChangeConsole(ConsoleColor.Yellow, $"Produkty z kategorii \"{category}\":\n----------------------------------------------------------------------------------------------------------------\n");

            sql_products_list.ForEach(p => p.Info());
            return sql_products_list;
        }
    }
}
