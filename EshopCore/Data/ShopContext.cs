using EshopCore.Models;
using EshopCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EshopCore.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext() { } 
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<CartItem> Cart { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite($"Data Source={Validators.SQL_DB_Path("shop.db")}");
                //.LogTo(Console.WriteLine); // To pokaże Ci każdy ruch bazy w konsoli!
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(0);
                entity.Property(e => e.Username).HasColumnOrder(1);
                entity.Property(e => e.Password).HasColumnOrder(2);
                entity.Property(e => e.Email).HasColumnOrder(3);
                entity.Property(e => e.Role).HasColumnOrder(4);
                entity.Property(e => e.CreatedAt).HasColumnOrder(5);
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(0);
                entity.Property(e => e.Name).HasColumnOrder(1);
                entity.Property(e => e.Category).HasColumnOrder(2);
                entity.Property(e => e.Price).HasColumnOrder(3);
                entity.Property(e => e.Stock).HasColumnOrder(4);
                entity.Property(e => e.Description).HasColumnOrder(5);
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(0);
                entity.Property(e => e.UserId).HasColumnOrder(1);
                entity.Property(e => e.TotalAmount).HasColumnOrder(2);
                entity.Property(e => e.Status).HasColumnOrder(3);
                entity.Property(e => e.CreatedAt).HasColumnOrder(4);
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(0);
                entity.Property(e => e.OrderId).HasColumnOrder(1);
                entity.Property(e => e.ProductId).HasColumnOrder(2);
                entity.Property(e => e.Quantity).HasColumnOrder(3);
                entity.Property(e => e.ItemTotalPrice).HasColumnOrder(4);
            });
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnOrder(0);
                entity.Property(e => e.ProductId).HasColumnOrder(1);
                entity.Property(e => e.Quantity).HasColumnOrder(2);
                entity.Property(e => e.UserId).HasColumnOrder(3);
            });
        }
    }
}
