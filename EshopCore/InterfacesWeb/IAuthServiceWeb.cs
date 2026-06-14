using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.InterfacesWeb
{
    public interface IAuthServiceWeb
    {
        // Logowanie użytkownika
        Task<User?> Login(string username, string password);    
    }
}
