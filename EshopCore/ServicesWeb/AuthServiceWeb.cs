using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EshopCore.ServicesWeb
{
    public class AuthServiceWeb : IAuthServiceWeb
    {
        private readonly ShopContext _context;
        public AuthServiceWeb(ShopContext context)
        {
            _context = context;
        }

        public async Task<User?> Login(string username, string password)
        {
            var sql_user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username == username.ToLower());

            if (sql_user == null)
            {
                return null;
            }
            var isPasswordValid = Validators.VerifyUserPassword(sql_user, password);

            if (!isPasswordValid)
            {
                return null;
            }
            return sql_user;
        }
    }
}
