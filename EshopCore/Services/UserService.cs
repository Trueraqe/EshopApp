using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EshopCore.Services
{
    public class UserService : IUserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly IValidator<User> _validator;
        private readonly ShopContext _context;
        public UserService(ShopContext context, IValidator<User> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task RegisterUser(string username, string email, string password)
        {
            var sql_user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username.ToLower() == username.ToLower() || u.Email.ToLower() == email.ToLower());

            if (sql_user == null)
            {
                var newUser = new User(username.ToLower(), email.ToLower(), password, Validators.ClearDateTime());

                var result = await _validator.ValidateAsync(newUser);

                if (!result.IsValid)
                {
                    Validators.ChangeConsole(ConsoleColor.Red, "Błędne dane.");
                    return;
                }
                newUser.Password = _passwordHasher.HashPassword(newUser, password);

                Validators.ChangeConsole(ConsoleColor.Green, $"Konto '{username}' zostało utworzone.");

                await _context.AddAsync(newUser);

                await _context.SaveChangesAsync();
                return;
            }
            Validators.ChangeConsole(ConsoleColor.Red, "Nazwa użytkownika/email są już zajęte lub dane rejestracji są nie prawidłowe.");
            return;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var sql_user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username == username.ToLower());

            if (sql_user == null)
            {
                Validators.ChangeConsole(ConsoleColor.Red, "Użytkownik nie istnieje.");
                return sql_user!;
            }
            sql_user.Info();
            return sql_user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            await _context.Users.AsNoTracking().ForEachAsync(u => u.Info());
            return _context.Users.ToList();
        }
    }
}
