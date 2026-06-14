using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Enums;
using EshopCore.Models;
using EshopCore.Interfaces;
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

namespace EshopCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShopContext _context;
        public OrderService(ShopContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(int userId)
        {
            var userCart = _context.Cart.Where(u => u.UserId == userId);

            if (userCart.Count() > 0)
            {
                if (Validators.Payment() == true)
                {
                    var cs = new CartService(_context);

                    var order = new Order(userId, Validators.ClearDateTime(), await cs.GetTotalCartPrice(userId));

                    await _context.AddAsync(order);

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

                    Validators.ChangeConsole(ConsoleColor.Green, "Zamówienie zostało złożone.");
                    return;
                }
            }
            Validators.ChangeConsole(ConsoleColor.Red, "Twój koszyk jest pusty.");
            return;
        }

        public async Task<List<Order>> LoggedInUserOdrersHistory(int userId)
        {
            var ordersList = _context.Orders.Include(o => o.Buyer).Where(o => o.Buyer.Id == userId).ToList();

            Validators.ChangeConsole(ConsoleColor.Cyan, "Historia zamówień:\n----------------------------------------------------------------------------------------------------------------\n");

            foreach (var order in ordersList)
            {
                Validators.ChangeConsole(ConsoleColor.Yellow, $"Data zamówienia: {order.CreatedAt} | Kupujący: {order.Buyer.Username} | Kwota całkowita: {order.TotalAmount}zł | Status: {order.Status}\n\nLista produktów:");

                var orderItemsWithProducts = _context.OrderItems.Include(item => item.Product).Where(item => item.OrderId == order.Id).ToList();

                foreach (var item in orderItemsWithProducts)
                {
                    if (item.Product != null)
                    {
                        Validators.ChangeConsole(ConsoleColor.Yellow, $"Nazwa: {item.Product.Name}, Kategoria: {item.Product.Category}, Cena: {item.Product.Price}zł/szt, Ilość: {item.Quantity}, Opis: {item.Product.Description}");
                    }
                }
                Validators.ChangeConsole(ConsoleColor.Yellow, $"\n----------------------------------------------------------------------------------------------------------------\n");
            }
            return ordersList;
        }

        public async Task<List<Order>> GetUserOrdersHistoryByUsername(string username)
        {
            var sql_user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

            if (sql_user == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Użytkownik o podanej nazwie już istnieje.");
                return new List<Order>();
            }
            Validators.ChangeConsole(ConsoleColor.Cyan, $"Zamówienia użytkownika \"{username.ToLower()}\"\n----------------------------------------------------------------------------------------------------------------");

            var ordersList = _context.Orders.Include(o => o.Buyer).Where(o => o.UserId == sql_user.Id).ToList();

            foreach (var order in ordersList)
            {
                var orderItemsWithProducts = _context.OrderItems.AsNoTracking().Include(item => item.Product).Where(item => item.OrderId == order.Id).ToList();

                Validators.ChangeConsole(ConsoleColor.Yellow, $"\nID: {order.Id}, Data zamówienia: {order.CreatedAt} | Kupujący: {order.Buyer.Username} | Kwota całkowita: {order.TotalAmount}zł | Status: {order.Status}\n\nLista produktów:");

                foreach (var item in orderItemsWithProducts)
                {
                    if (item.Product != null)
                    {
                        Validators.ChangeConsole(ConsoleColor.Yellow, $"Nazwa: {item.Product.Name}, Kategoria: {item.Product.Category}, Cena: {item.Product.Price}zł/szt, Ilość: {item.Quantity}");
                    }
                }
                Validators.ChangeConsole(ConsoleColor.Yellow, $"\n----------------------------------------------------------------------------------------------------------------");
            }
            return ordersList;
        }

        public async Task<List<Order>> GetAllUsersOrders()
        {
            var orders = _context.Orders.AsNoTracking().ToList();

            Validators.ChangeConsole(ConsoleColor.Cyan, "Historia zamówień wszystkich użytkowników:\n----------------------------------------------------------------------------------------------------------------");

            foreach (var order in _context.Orders)
            {
                var sql_user = _context.Users.AsNoTracking().First(u => u.Id == order.UserId);

                var sql_order = _context.Orders.AsNoTracking().First(o => o.Id == order.Id && o.UserId == order.UserId);

                Validators.ChangeConsole(ConsoleColor.Yellow, $"\nID zamówienia: {sql_order.Id} | Data zamówienia: {sql_order.CreatedAt} | Kupujący: {sql_user.Username} | Kwota całkowita: {sql_order.TotalAmount}zł | Status: {sql_order.Status}\n\nLista produktów:");

                foreach (var item in _context.OrderItems.Where(o => o.OrderId == order.Id))
                {
                    var product = _context.Products.AsNoTracking().First(p => p.Id == item.ProductId);

                    Validators.ChangeConsole(ConsoleColor.Yellow, $"Nazwa: {product.Name}, Kategoria: {product.Category}, Cena: {product.Price}zł/szt, Ilość: {item.Quantity}");
                }
                Validators.ChangeConsole(ConsoleColor.Yellow, $"\n----------------------------------------------------------------------------------------------------------------");
            }
            return orders;
        }

        public async Task ChangeOrderStatus(int orderId)
        {
            var sql_order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (sql_order == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Zamówienie o podanym ID nie istnieje.");
                return;
            }
            await Validators.OrderStatusChange(sql_order);

            await _context.SaveChangesAsync();
            return;
        }
    }
}
