using EshopCore.Enums;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EshopCore.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }
        public string CreatedAt { get; set; }

        public List<Order> Orders { get; set; } = new();

        private User() { }
        public User(string username, string email, string password, string createdAt)
        {
            Username = username;
            Email = email;
            Password = password;
            Role = UserRole.Customer;
            CreatedAt = createdAt;
        }
        public void Info()
        {
            Validators.ChangeConsole(ConsoleColor.Yellow, $"ID: {Id}, Nazwa: {Username}, Email: {Email}, Rola: {Role}, Data utworzenia: {CreatedAt}");
        }
    }
}
