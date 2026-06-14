using EshopCore.Database;
using EshopCore.Enums;
using EshopCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User Buyer { get; set; } = null!;

        public string CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public List<OrderItem> Items { get; set; } = new();

        private Order() { }
        public Order(int userId, string createdAt, decimal totalAmount)
        {
            UserId = userId;
            CreatedAt = createdAt;
            TotalAmount = totalAmount;
            Status = OrderStatus.New;
        }
        public void New()
        {
            Status = OrderStatus.New;
        }
        public void Shipped()
        {
            Status = OrderStatus.Shipped;
        }
        public void Cancelled()
        {
            Status = OrderStatus.Cancelled;
        }
        public void Delivered()
        {
            Status = OrderStatus.Delivered;
        }
        public void Info()
        {
            Validators.ChangeConsole(ConsoleColor.Yellow, $"ID: {Id}, Kupujący: {UserId}, Koszt: {TotalAmount}zł, Status: {Status}, Data: {CreatedAt}");
        }
    }
}
