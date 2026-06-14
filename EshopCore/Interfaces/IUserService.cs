using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Interfaces
{
    public interface IUserService
    {
        // Rejestracja użytkownika
        Task RegisterUser(string username, string email, string password);

        // Wyświetlanie danych użytkownika
        Task<User> GetUserByUsername(string username);

        // Wyświetlanie danych wszystkich użytkowników
        Task<List<User>> GetAllUsers();
    }
}
