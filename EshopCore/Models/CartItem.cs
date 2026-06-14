using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        private CartItem() { }
        public CartItem(int userId, int productId, int quantity)
        {
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }

        public void Info()
        {
            Validators.ChangeConsole(ConsoleColor.Yellow, $"Produkt: {Product}, Ilość: {Quantity}");
        }
    }
}