using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class CartServiceTests
{
    [Fact]
    public async Task CartTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IValidator<User> _validator = new RegisterUserValidator();
        IProductService ps = new ProductService(_context);
        ICartService cs = new CartService(_context);
        IUserService us = new UserService(_context, _validator);

        await us.RegisterUser("user1", "user1@email", "useR1?123");
        await us.RegisterUser("user2", "user2@email", "useR2?123");
        await ps.AddProduct("product1", "category1", 10, 20, "description1");
        await ps.AddProduct("product2", "category2", 30, 40, "description2");
        await ps.AddProduct("product3", "category3", 50, 60, "description3");
        await ps.AddProduct("product4", "category4", 70, 80, "description4");

        // AddProductToCart
        await cs.AddProductToCart(1, 1, 10);
        await cs.AddProductToCart(2, 3, 50);

        Assert.NotEmpty(_context.Cart.Where(u => u.UserId == 1).ToList());
        Assert.NotEmpty(_context.Cart.Where(u => u.UserId == 2).ToList());
        Assert.Empty(_context.Cart.Where(u => u.UserId == 3).ToList());
        Assert.False(await cs.AddProductToCart(1, 1, 11));
        Assert.False(await cs.AddProductToCart(1, 5, 11));

        // RemoveProductFromCart

        Assert.True(await cs.RemoveProductFromCart(2, 3, 50));
        Assert.Empty(_context.Cart.Where(u => u.UserId == 2).ToList());
        Assert.False(await cs.RemoveProductFromCart(2, 5, 50));
        Assert.False(await cs.RemoveProductFromCart(2, 2, 50));
        Assert.False(await cs.RemoveProductFromCart(1, 1, 20));
        Assert.True(await cs.RemoveProductFromCart(1, 1, 1));

        // GetTotalCartPrice
        var user = _context.Users.First();

        Assert.Equal(90, await cs.GetTotalCartPrice(user.Id));

        // ClearUserCart
        await cs.ClearUserCart(1);
        Assert.Empty(_context.Cart.Where(u => u.UserId == 1).ToList());

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}