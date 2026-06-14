using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using EshopCore.Data;

public static class TestDbFactory
{
    public static ShopContext CreateContext(out SqliteConnection connection)
    {
        // 1. Tworzymy SQLite in-memory
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        // 2. Konfigurujemy DbContext
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ShopContext(options);

        // 3. Tworzymy schemat bazy (tabele)
        context.Database.EnsureCreated();

        return context;
    }
}