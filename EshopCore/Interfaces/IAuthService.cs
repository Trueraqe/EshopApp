using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Interfaces
{
    public interface IAuthService
    {
        // Logowanie użytkownika
        User? Login(string username, string password);

        // Wylogowanie użytkownika
        User? Logout();

        // Wyświetlanie nazwy aktualnie zalogowanego użytkownika
        User CurrentUser();
    }
}
