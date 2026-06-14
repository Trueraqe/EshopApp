using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Enums;
using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using EshopCore.InterfacesWeb;

namespace EshopCore.ServicesWeb
{
    public class OrderServiceWeb : IOrderServiceWeb
    {
        private readonly ShopContext _context;
        public OrderServiceWeb(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrder(int userId)
        {
            var userCart = _context.Cart.Where(u => u.UserId == userId);

            if (userCart.Count() > 0)
            {
                var cs = new CartServiceWeb(_context);

                var order = new Order(userId, Validators.ClearDateTime(), await cs.GetTotalCartPrice(userId));

                _context.Add(order);

                await _context.SaveChangesAsync();

                foreach (var cart_item in userCart)
                {
                    var sql_product = _context.Products.FirstOrDefault(p => p.Id == cart_item.ProductId);

                    if (sql_product != null)
                    {
                        sql_product.Stock -= cart_item.Quantity;

                        var order_item = new OrderItem(sql_product.Price, order.Id, cart_item.ProductId, cart_item.Quantity);

                        await _context.AddAsync(order_item);

                        _context.Remove(cart_item);
                    }
                }
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Order>> LoggedInUserOdrersHistory(int userId)
        {
            return await _context.Orders.AsNoTracking().Where(o => o.UserId == userId).Include(oi => oi.Items).ThenInclude(oi => oi.Product).ToListAsync();
        }

        public async Task<List<Order>> GetUserOrdersHistoryByUsername(string username)
        {
            return await _context.Orders.AsNoTracking().Where(o => o.Buyer.Username.ToLower() == username.ToLower()).Include(o => o.Items).ThenInclude(oi => oi.Product).ToListAsync();
        }

        public async Task<List<Order>> GetAllUsersOrders()
        {
            return await _context.Orders.AsNoTracking().Include(o => o.Buyer).Include(o => o.Items).ThenInclude(o => o.Product).ToListAsync();
        }

        public async Task<bool> ChangeOrderStatus(int orderId, OrderStatus newStatus)
        {
            var sql_order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (sql_order == null)
            {
                return false;
            }
            sql_order.Status = newStatus;

            _context.Update(sql_order);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
