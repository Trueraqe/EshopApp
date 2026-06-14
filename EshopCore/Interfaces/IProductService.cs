using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Interfaces
{
    public interface IProductService
    {
        // Dodawanie nowego produktu do bazy
        Task AddProduct(string name, string category, decimal price, int stock, string description);

        // Usuwanie produktu z bazy
        Task RemoveProduct(int id);

        // Aktualizowanie produktu w bazie
        Task UpdateProduct(int id, string name, string category, decimal price, int stock, string description);

        // Wyświetlanie wszystkich produktów z bazy
        Task<List<Product>> GetAllProducts();

        // Wyświetlanie produktu o podanej nazwie
        Task<Product> SearchProductByName(string name);
        
        // Wyświetlanie wszystkich produktów o podanej kategorii
        Task<List<Product>> FilterProductsByCategory(string category);
    }
}
