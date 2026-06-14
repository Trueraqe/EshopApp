using EshopCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.InterfacesWeb
{
    public interface IProductServiceWeb
    {
        // Dodawanie nowego produktu do bazy
        Task<bool> AddProduct(Product product);

        // Usuwanie produktu z bazy
        Task<bool> RemoveProduct(Product product);

        // Aktualizowanie produktu w bazie
        Task<bool> UpdateProduct(Product product);

        // Wyświetlanie wszystkich produktów z bazy
        Task<List<Product>> GetAllProducts();

        // Wyświetlanie produktu o podanej nazwie
        Task<Product?> SearchProductByName(string name);

        // Wyświetlanie wszystkich produktów o podanej kategorii
        Task<List<Product>?> FilterProductsByCategory(string category);
    }
}
