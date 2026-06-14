using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Enums;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.ServicesWeb;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class OrderServiceWebTests
{
    [Fact]
    public async Task OrdersWebTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IValidator<User> _validator = new RegisterUserValidator();
        IProductServiceWeb ps = new ProductServiceWeb(_context);
        ICartServiceWeb cs = new CartServiceWeb(_context);
        IOrderServiceWeb os = new OrderServiceWeb(_context);
        IUserServiceWeb us = new UserServiceWeb(_context, _validator);

        Product product_1 = new("product1", "category1", 10, 20, "description1");
        Product product_2 = new("product2", "category2", 30, 40, "description2");
        Product product_3 = new("product3", "category3", 50, 60, "description3");
        Product product_4 = new("product4", "category4", 70, 80, "description4");

        await us.RegisterUser("user1", "user1@email", "useR1?123");
        await us.RegisterUser("user2", "user2@email", "useR2?123");
        await ps.AddProduct(product_1);
        await ps.AddProduct(product_2);
        await ps.AddProduct(product_3);
        await ps.AddProduct(product_4);

        // AddProductToCart
        await cs.AddProductToCart(1, 1, 10);
        await cs.AddProductToCart(1, 2, 20);
        await cs.AddProductToCart(1, 3, 30);
        await cs.AddProductToCart(1, 4, 40);

        // CreateOrder
        var user = _context.Users.First();
        await os.CreateOrder(user.Id);

        var order = _context.Orders.First();

        Assert.Equal(1, order.Id);
        Assert.Equal(1, order.UserId);
        Assert.Equal(5000, order.TotalAmount);
        Assert.Equal(OrderStatus.New, order.Status);

        var orderItem = _context.OrderItems.ToList();

        Assert.Equal(4, orderItem[3].Id);
        Assert.Equal(70, orderItem[3].ItemTotalPrice);
        Assert.Equal(1, orderItem[3].OrderId);
        Assert.Equal(4, orderItem[3].ProductId);
        Assert.Equal(40, orderItem[3].Quantity);

        // LoggedInUserOdrersHistory
        var orderHistory = await os.LoggedInUserOdrersHistory(1);
        var orderHistory_v2 = await os.LoggedInUserOdrersHistory(2);

        Assert.NotEmpty(orderHistory);
        Assert.Empty(orderHistory_v2);

        // GetUserOrdersByName
        var userOrders = await os.GetUserOrdersHistoryByUsername("user1");
        var userOrders_v2 = await os.GetUserOrdersHistoryByUsername("user2");

        Assert.NotEmpty(userOrders!);
        Assert.Empty(userOrders_v2!);

        // GetAllUsersOrders
        var allOrders = await os.GetAllUsersOrders();

        Assert.NotEmpty(allOrders);

        // ChangeStatus
        await os.ChangeOrderStatus(1, OrderStatus.Cancelled);
        var order_v2 = _context.Orders.First();

        Assert.Equal(OrderStatus.Cancelled, order_v2.Status);

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}