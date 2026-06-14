using EshopCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static EshopCore.Helpers.Helpers;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EshopCore.Helpers
{
    public class Helpers
    {
        // Database commands 
        // Add-Migration Init //Add-Migration CartItem
        // Update-Database

        //Clear PRIMARY KEY AUTOINCREMENT
        //DELETE FROM sqlite_sequence WHERE name='Cart';
        //DELETE FROM sqlite_sequence WHERE name='Users';
        //DELETE FROM sqlite_sequence WHERE name='Products';
        //DELETE FROM sqlite_sequence WHERE name='Orders';
        //DELETE FROM sqlite_sequence WHERE name='OrderItems';
        //SELECT * FROM sqlite_master;

        // Swagger libary
        // Swashbuckle.AspNetCore
        // Port niższy (np. 5000 lub 5xxx): Obsługuje standardowy, nieszyfrowany protokół HTTP.
        // Port wyższy(np. 7001 lub 7xxx): Obsługuje bezpieczny, szyfrowany protokół HTTPS

        // JWT
        // Klucz (HS256) musi mieć minimum 256 bitów = 32 znaki
    }
}