using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EshopCore.ServicesWeb
{
    public class UserServiceWeb : IUserServiceWeb
    {
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly IValidator<User> _validator;
        private readonly ShopContext _context;
        public UserServiceWeb(ShopContext context, IValidator<User> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<bool> RegisterUser(string username, string email, string password)
        {
            var sql_user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username.ToLower() == username.ToLower() || u.Email.ToLower() == email.ToLower());

            if (sql_user == null)
            {
                var newUser = new User(username.ToLower(), email.ToLower(), password, Validators.ClearDateTime());

                var result = await _validator.ValidateAsync(newUser);

                if (!result.IsValid)
                {
                    return false;
                }
                newUser.Password = _passwordHasher.HashPassword(newUser, password);

                await _context.AddAsync(newUser);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<User>> GetUserByUsername(string username)
        {
            return await _context.Users.AsNoTracking().Where(u => u.Username.Contains(username.ToLower())).ToListAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
    }
}
