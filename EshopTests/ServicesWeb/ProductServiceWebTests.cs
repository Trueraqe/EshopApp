using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.ServicesWeb;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProductServiceWebTests
{
    [Fact]
    public async Task ProductsWebTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IProductServiceWeb ps = new ProductServiceWeb(_context);

        // AddProduct
        Product product_1 = new("product1", "category1", 10, 20, "description1");
        Product product_2 = new("product2", "category2", 30, 40, "description2");
        Product product_3 = new("product3", "category3", 50, 60, "description3");
        Product product_4 = new("product4", "category4", 70, 80, "description4");

        await ps.AddProduct(product_1);
        await ps.AddProduct(product_2);
        await ps.AddProduct(product_3);
        await ps.AddProduct(product_4);

        var allProducts = _context.Products.ToList();

        Assert.Equal(4, allProducts.Count());

        // RemoveProduct
        var item = _context.Products.First();
        await ps.RemoveProduct(item);

        var allProducts_v2 = _context.Products.ToList();
        Assert.Equal(3, allProducts_v2.Count());

        // UpdateProduct
        var item_v2 = _context.Products.First();
        item_v2.Name = "product1_updated";
        item_v2.Category = "category1_updated";
        item_v2.Price = 1000;
        item_v2.Stock = 2000;
        item_v2.Description = "description1_updated";

        await ps.UpdateProduct(item_v2);

        Assert.Equal(1000, allProducts_v2[0].Price);
        Assert.Equal(2000, allProducts_v2[0].Stock);
        Assert.Equal("product1_updated", allProducts_v2[0].Name);
        Assert.Equal("category1_updated", allProducts_v2[0].Category);
        Assert.Equal("description1_updated", allProducts_v2[0].Description);

        // GetAllProducts
        var allProducts_v3 = await ps.GetAllProducts();

        Assert.Equal(3, allProducts_v3.Count());

        // SearchProductByName
        var productByName = await ps.SearchProductByName("nullcheck");
        var productByName_v2 = await ps.SearchProductByName("product3");

        Assert.Null(productByName);
        Assert.NotNull(productByName_v2);

        // FilterProductsByCategory
        var productsByCategory = await ps.FilterProductsByCategory("nullcheck");
        var productsByCategory_v2 = await ps.FilterProductsByCategory("category3");

        Assert.Null(productsByCategory);
        Assert.NotNull(productsByCategory_v2);

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}