using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace EshopCore.Services
{
    public class AuthService : IAuthService
    {
        public static User? authenticated;
        private readonly ShopContext _context;
        public AuthService(ShopContext context)
        {
            _context = context;
        }

        public User? Login(string username, string password)
        {
            var sql_user = _context.Users.FirstOrDefault(u => u.Username == username.ToLower());

            if (sql_user == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Niepoprawny login lub hasło.");
                return null;
            }
            var isPasswordValid = Validators.VerifyUserPassword(sql_user, password);

            if (!isPasswordValid)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Niepoprawny login lub hasło.");
                return null;
            }
            authenticated = sql_user;

            Validators.ChangeConsole(ConsoleColor.Green, "Zostałeś zalogowany.");

            return authenticated;
        }

        public User? Logout()
        {
            Validators.ChangeConsole(ConsoleColor.DarkMagenta, $"Zostałeś wylogowany {authenticated!.Username}.");

            authenticated = null;

            return authenticated;
        }

        public User CurrentUser()
        {
            if (authenticated != null)
            {
                Validators.ChangeConsole(ConsoleColor.DarkMagenta, $"Bieżący użytkownik: {authenticated.Username}");
            }
            return authenticated!;
        }
    }
}
