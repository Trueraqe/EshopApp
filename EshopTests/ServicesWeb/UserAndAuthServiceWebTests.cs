using EshopCore.Data;
using EshopCore.Database;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.ServicesWeb;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserAndAuthServiceWebTests
{
    [Fact]
    public async Task UsersAndAuthWebTests()
    {
        using var _context = TestDbFactory.CreateContext(out var connection);

        IValidator<User> _validator = new RegisterUserValidator();
        IUserServiceWeb us = new UserServiceWeb(_context, _validator);
        IAuthServiceWeb auth = new AuthServiceWeb(_context);

        // RegisterUser
        await us.RegisterUser("user1", "user1@email", "useR1?123");
        await us.RegisterUser("user2", "user2@email", "useR2?123");
        var wrongEmailUser = await us.RegisterUser("user3", "user3email", "useR2?123");

        var registered = _context.Users;

        Assert.Equal(2, registered.Count());
        Assert.False(wrongEmailUser);

        // GetByUsername
        var userByName = await us.GetUserByUsername("nullcheck");
        var userByName_v2 = await us.GetUserByUsername("user1");

        Assert.Empty(userByName);
        Assert.NotEmpty(userByName_v2);

        // GetAllUsers
        var allUsers = await us.GetAllUsers();

        Assert.Equal(2, allUsers.Count());

        // Login
        var loggedUser = await auth.Login("user2", "useR2?123");
        var notLoggedUser = await auth.Login("nullcheck", "nullcheck");

        Assert.NotNull(loggedUser);
        Assert.Null(notLoggedUser);

        // Zamykanie bazy
        await connection.DisposeAsync();
    }
}