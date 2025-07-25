using E_commerce.Domain.Enums;
using E_commerce.Domain.Models;
using E_commerce.Infrastructure.Contexts;
using E_commerce.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTests.Controller.Helpers
{
    public static class TestDbContextFactory
    {
        public static async Task<ApplicationDbContext> CreateEmptyContextAsync(string testName)
        {
            var dbName = testName is null
                ? Guid.NewGuid().ToString()
                : $"{testName}_{Guid.NewGuid()}";

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }
        public static async Task<ApplicationDbContext> CreateWithBasicOrdersAsync(string testName = null)
        {
            var context = await CreateEmptyContextAsync(testName);


            var role = new Role { Name = UserRole.User.ToString() };
            var category = new Category { Name = "Electrónica" };

            var prod1 = new Product
            {
                Name = "Auriculares",
                Price = 50m,
                Stock = 100,
                Category = category
            };
            var prod2 = new Product
            {
                Name = "Teclado",
                Price = 80m,
                Stock = 50,
                Category = category
            };


            var user = new User
            {
                Username = "testuser",
                Email = "user@test.com",
                Password = "hashedpassword",
                role = role
            };


            var order = new Order
            {
                User = user,
                Status = OrderStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                IsExpress = false,
                Items = new List<OrderItem>()
            };
            order.Items.Add(new OrderItem
            {
                Order = order,
                Product = prod1,
                Quantity = 2,
                UnitPrice = prod1.Price
            });
            order.Items.Add(new OrderItem
            {
                Order = order,
                Product = prod2,
                Quantity = 1,
                UnitPrice = prod2.Price
            });


            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return context;
        }



    }
}
