using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.InterfacesWeb
{
    public interface IUserServiceWeb
    {
        // Rejestracja użytkownika
        Task<bool> RegisterUser(string username, string email, string password);

        // Wyświetlanie danych użytkownika
        Task<List<User>> GetUserByUsername(string username);

        // Wyświetlanie danych wszystkich użytkowników
        Task<List<User>> GetAllUsers();
    }
}
