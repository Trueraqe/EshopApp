using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public async Task ProductsTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IProductService ps = new ProductService(_context);

        // AddProduct
        await ps.AddProduct("product1", "category1", 100, 200, "description1");
        await ps.AddProduct("product2", "category2", 300, 400, "description2");
        await ps.AddProduct("product3", "category3", 500, 600, "description3");
        await ps.AddProduct("product4", "category4", 700, 800, "description4");

        var products = _context.Products.ToList();

        Assert.Equal(4, products.Count);

        // RemoveProduct
        await ps.RemoveProduct(3);

        var products_v2 = _context.Products.ToList();

        Assert.Equal(3, products_v2.Count);

        // UpdateProduct
        await ps.UpdateProduct(1, "product1_updated", "category1_updated", 1000, 2000, "description1_updated");

        Assert.Equal(1000, products_v2[0].Price);
        Assert.Equal(2000, products_v2[0].Stock);
        Assert.Equal("product1_updated", products_v2[0].Name);
        Assert.Equal("category1_updated", products_v2[0].Category);
        Assert.Equal("description1_updated", products_v2[0].Description);

        // GetAllProducts
        var products_v3 = await ps.GetAllProducts();

        Assert.NotEmpty(products_v3);

        // SearchProductByName
        var product = await ps.SearchProductByName("nullcheck");
        var product_v2 = await ps.SearchProductByName("product2");

        Assert.Null(product);
        Assert.NotNull(product_v2);

        // FilterProductsByCategory
        var products_v4 = await ps.FilterProductsByCategory("nullcheck");
        var products_v5 = await ps.FilterProductsByCategory("category1_updated");

        Assert.Empty(products_v4);
        Assert.NotEmpty(products_v5);

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}