using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
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

namespace EshopCore.ServicesWeb
{
    public class CartServiceWeb : ICartServiceWeb
    {
        private readonly ShopContext _context;
        public CartServiceWeb(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProductToCart(int userId, int productId, int quantity)
        {
            var cartProduct = _context.Cart.FirstOrDefault(p => p.UserId == userId && p.ProductId == productId);

            var sql_product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == productId);

            if (sql_product == null)
            {
                return false;
            }
            int quantityInCart = _context.Cart.Where(i => i.ProductId == productId && i.UserId == userId).Sum(i => i.Quantity);

            if (sql_product.Stock < (quantityInCart + quantity))
            {
                return false;
            }
            if (cartProduct != null)
            {
                cartProduct.Quantity += quantity;

                _context.Update(cartProduct);

                await _context.SaveChangesAsync();
                return true;
            }
            var newItem = new CartItem(userId, productId, quantity);

            await _context.AddAsync(newItem);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProductFromCart(int userId, int productId, int quantity)
        {
            var cartProduct = _context.Cart.FirstOrDefault(p => p.UserId == userId && p.ProductId == productId);

            var sql_product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (sql_product == null || cartProduct == null)
            {
                return false;
            }
            if (cartProduct.Quantity == quantity)
            {
                _context.Remove(cartProduct);

                await _context.SaveChangesAsync();
                return true;
            }
            else if (cartProduct.Quantity < quantity)
            {
                return false;
            }
            cartProduct.Quantity -= quantity;

            _context.Update(cartProduct);

            await _context.SaveChangesAsync();
            return true;
        }

        public Task ClearUserCart(int userId)
        {
            _context.Cart.Where(u => u.UserId == userId).ExecuteDelete();

            return Task.CompletedTask;
        }

        public async Task<decimal> GetTotalCartPrice(int userId)
        {
            decimal cost = 0;

            var cart = await _context.Cart.AsNoTracking().Where(i => i.UserId == userId).Include(p => p.Product).ToListAsync();

            foreach (var item in cart)
            {
                cost += item.Product.Price * item.Quantity;
            }
            return cost;
        }

        public async Task<List<CartItem>> GetUserCartItems(int userId)
        {
            return await _context.Cart.AsNoTracking().Where(i => i.UserId == userId).Include(p => p.Product).ToListAsync();
        }
    }
}
