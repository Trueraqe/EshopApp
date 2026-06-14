using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EshopCore.ServicesWeb
{
    public class ProductServiceWeb : IProductServiceWeb
    {
        private readonly ShopContext _context;
        public ProductServiceWeb(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Category) || decimal.IsNegative(product.Price) || int.IsNegative(product.Stock) || string.IsNullOrWhiteSpace(product.Description))
            {
                return false;
            }
            var new_product = new Product(product.Name, product.Category, product.Price, product.Stock, product.Description);

            await _context.AddAsync(new_product);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProduct(Product product)
        {
            var sql_product = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            if (sql_product == null)
            {
                return false;
            }
            _context.Remove(sql_product);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Category) || decimal.IsNegative(product.Price) || int.IsNegative(product.Stock) || string.IsNullOrWhiteSpace(product.Description))
            {
                return false;
            }            
            var sql_product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);

            if (sql_product == null)
            {
                return false;
            }
            _context.Update(product);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> SearchProductByName(string name)
        {
            var sql_product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Name.ToLower() == name.ToLower());

            if (sql_product == null)
            {
                return null;
            }
            return sql_product;
        }

        public async Task<List<Product>?> FilterProductsByCategory(string category)
        {
            var sql_products_list = await _context.Products.AsNoTracking().Where(p => p.Category.ToLower() == category.ToLower()).ToListAsync();

            if (!sql_products_list.Any())
            {
                return null;
            }
            return sql_products_list;
        }
    }
}
