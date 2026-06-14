using EshopCore.Data;
using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.Database
{
    public class FakeDatabase
    {
        public static List<User> Users = new();
        public static List<Product> Products = new();
        public static List<Order> Orders = new();
    }
}
