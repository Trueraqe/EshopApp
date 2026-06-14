using EshopCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = null!;

        public List<OrderItem> OrderItems { get; set; } = new();

        private Product() { }
        public Product(string name, string category, decimal price, int stock, string description)
        {
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
            Description = description;
        }
        public void Info()
        {
            Validators.ChangeConsole(ConsoleColor.Yellow, $"ID: {Id}, Nazwa: {Name}, Kategoria: {Category}, Cena: {Price}zł, Na stanie: {Stock}, Opis: {Description}");
        }
    }
}
