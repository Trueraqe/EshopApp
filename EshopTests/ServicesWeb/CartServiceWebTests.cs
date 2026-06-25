using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.ServicesWeb;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
public class CartServiceWebTests
{
    [Fact]
    public async Task CartWebTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IValidator<User> _validator = new RegisterUserValidator();
        IProductServiceWeb ps = new ProductServiceWeb(_context);
        ICartServiceWeb cs = new CartServiceWeb(_context);
        IUserServiceWeb us = new UserServiceWeb(_context, _validator);

        Product product_1 = new("product1", "category1", 10, 20, "description1");
        Product product_2 = new("product2", "category2", 30, 40, "description2");
        Product product_3 = new("product3", "category3", 50, 60, "description3");
        Product product_4 = new("product4", "category4", 70, 80, "description4");

        await us.RegisterUser("user1", "user1@email", "useR1?123", "useR1?123");
        await us.RegisterUser("user2", "user2@email", "useR2?123", "useR2?123");
        await ps.AddProduct(product_1);
        await ps.AddProduct(product_2);
        await ps.AddProduct(product_3);
        await ps.AddProduct(product_4);

        // AddProductToCart
        var addProduct = await cs.AddProductToCart(1, 1, 10);
        var addProduct_v2 = await cs.AddProductToCart(2, 3, 50);
        var notEnoughStockQuantity = await cs.AddProductToCart(1, 1, 11);
        var notEnoughStockQuantity_v2 = await cs.AddProductToCart(2, 3, 11);

        Assert.True(addProduct);
        Assert.True(addProduct_v2);
        Assert.False(notEnoughStockQuantity);
        Assert.False(notEnoughStockQuantity_v2);

        var userCartItems = _context.Cart.Where(u => u.UserId == 1).ToList();
        var userCartItems_v2 = _context.Cart.Where(u => u.UserId == 2).ToList();
        var userCartItems_v3 = _context.Cart.Where(u => u.UserId == 3).ToList();

        Assert.NotEmpty(userCartItems);
        Assert.NotEmpty(userCartItems_v2);
        Assert.Empty(userCartItems_v3);

        // RemoveProductFromCart
        var remove = await cs.RemoveProductFromCart(2, 3, 49);
        var userCart = _context.Cart.Where(u => u.UserId == 2).ToList();

        Assert.True(remove);
        Assert.NotEmpty(userCart);

        var remove_v2 = await cs.RemoveProductFromCart(2, 3, 2);
        var userCart_v2 = _context.Cart.Where(u => u.UserId == 2).ToList();

        Assert.False(remove_v2);
        Assert.NotEmpty(userCart_v2);

        var remove_v3 = await cs.RemoveProductFromCart(2, 3, 1);
        var userCart_v3 = _context.Cart.Where(u => u.UserId == 2).ToList();

        Assert.True(remove_v3);
        Assert.Empty(userCart_v3);

        // GetTotalCartPrice
        var user = _context.Users.First();
        var userCartTotalPrice = await cs.GetTotalCartPrice(user.Id);

        Assert.Equal(100, userCartTotalPrice);

        // ClearUserCart
        var userCart_v4 = _context.Cart.Where(u => u.UserId == 1).ToList();
        Assert.NotEmpty(userCart_v4);

        await cs.ClearUserCart(1);

        var userCart_v5 = _context.Cart.Where(u => u.UserId == 1).ToList();
        Assert.Empty(userCart_v5);

        // GetUserCartItems
        var userCart_v6 = await cs.GetUserCartItems(1);

        Assert.Empty(userCart_v6);

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}