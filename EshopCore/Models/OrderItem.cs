using EshopCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal ItemTotalPrice { get; set; }

        private OrderItem() { }
        public OrderItem(decimal itemTotalPrice, int orderId, int productId, int quantity)
        {
            ItemTotalPrice = itemTotalPrice;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
        public void Info()
        {
            Validators.ChangeConsole(ConsoleColor.Yellow, $"OrderId: {OrderId}, Produkt: {ProductId}, Ilość: {Quantity}, Cena za całość: {ItemTotalPrice}zł");
        }
    }
}
