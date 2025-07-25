using E_commerce.Domain.Enums;
using E_commerce.Domain.Models;
using E_commerce.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Infrastructure.Seeding
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {

            if (await _context.Roles.AnyAsync())
                return;


            _context.Roles.AddRange(
                new Role { Id = 1, Name = "admin" },
                new Role { Id = 2, Name = "user" }
            );


            _context.Categories.AddRange(
                new Category { Id = 1, Name = "Electrónica" },
                new Category { Id = 2, Name = "Ropa" },
                new Category { Id = 3, Name = "Hogar y Cocina" },

                new Category { Id = 10, Name = "Salud y Bienestar" }
            );


            _context.Products.AddRange(
                new Product { Id = 1, Name = "Smartphone X1", Price = 699.99m, Stock = 25, CategoryId = 1 },
                new Product { Id = 2, Name = "Televisor 4K 55”", Price = 499.50m, Stock = 10, CategoryId = 1 },

                new Product { Id = 11, Name = "Pulsera Inteligente Fit", Price = 129.99m, Stock = 22, CategoryId = 10 }
            );


            _context.Orders.AddRange(
                new Order
                {
                    Id = 1,
                    UserId = 1,
                    CreatedAt = new DateTime(2025, 5, 14, 10, 30, 0, DateTimeKind.Utc),
                    Status = OrderStatus.Placed,
                    IsExpress = false
                },
                new Order
                {
                    Id = 2,
                    UserId = 2,
                    CreatedAt = new DateTime(2025, 5, 15, 14, 45, 0, DateTimeKind.Utc),
                    Status = OrderStatus.Shipped,
                    IsExpress = true
                }
            );


            _context.Items.AddRange(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 699.99m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 6, Quantity = 2, UnitPrice = 14.95m },
                new OrderItem { Id = 3, OrderId = 2, ProductId = 3, Quantity = 3, UnitPrice = 29.99m },
                new OrderItem { Id = 4, OrderId = 2, ProductId = 10, Quantity = 1, UnitPrice = 79.99m }
            );

            await _context.SaveChangesAsync();
        }
    }
}
