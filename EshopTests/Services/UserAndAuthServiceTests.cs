using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserAndAuthServiceTests
{
    [Fact]
    public async Task UsersAndAuthTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IValidator<User> _validator = new RegisterUserValidator();
        IUserService us = new UserService(_context, _validator);
        IAuthService auth = new AuthService(_context);

        // RegisterUser
        await us.RegisterUser("user1", "user1@email", "useR1?123");
        await us.RegisterUser("user2", "user2@email", "useR2?123");

        var users = _context.Users.ToList();

        Assert.Equal(2, users.Count());

        // GetUserByUsername
        var user = await us.GetUserByUsername("nullcheck");
        var user_v2 = await us.GetUserByUsername("user1");

        Assert.Null(user);
        Assert.NotNull(user_v2);

        // GetAllUsers
        var users_v2 = await us.GetAllUsers();

        // Login
        var user_v3 = auth.Login("user2", "useR2?123");
        var user_v4 = auth.Login("nullcheck", "nullcheck");

        Assert.NotNull(user_v3);
        Assert.Null(user_v4);

        // Logout
        var user_logged_out = auth.Logout();

        Assert.Null(user_logged_out);

        // CurrentUser
        Assert.Null(auth.CurrentUser());

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}