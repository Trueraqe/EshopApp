using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Interfaces
{
    public interface ICartService
    {
        // Dodawanie produktu do koszyka użytkownika
        Task<bool> AddProductToCart(int userId, int productId, int quantity);

        // Usuwanie produktu z koszyka użytkownika
        Task<bool> RemoveProductFromCart(int userId, int productId, int quantity);

        // Usuwanie wszystkich produktów z koszyka użytkownika
        Task ClearUserCart(int userId);

        // Łączny koszt produktów w koszyku
        Task<decimal> GetTotalCartPrice(int userId);
    }
}
