using E_commerce.Domain.Enums;
using E_commerce.Domain.Models;
using E_commerce.Infrastructure.Repositories;
using E_commerceTests.Controller.Helpers;
using FluentAssertions;

namespace E_commerceTests.Controller
{
    public class OrdenRepositoryTests
    {
        [Fact]

        public async Task Create_ShouldAddOrderToDatabase()
        {
            //Arrange
            var context = await TestDbContextFactory.CreateEmptyContextAsync(nameof(GetAll_ShouldReturnAllOrders));
            var repository = new OrderRepository(context);
            var order = new Order
            {
                Status = OrderStatus.Placed,
                IsExpress = false,
                CreatedAt = DateTime.UtcNow,
                UserId = 1,
                Items = new List<OrderItem>()
            };


            //Act
            var result = await repository.Create(order);
            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Status.Should().Be(OrderStatus.Placed);
            result.UserId.Should().Be(1);

            var exists = await context.Orders.FindAsync(result.Id);
            exists.Should().NotBeNull();
        }
        [Fact]
        public async Task GetAll_ShouldReturnAllOrders()
        {
            var context = await TestDbContextFactory.CreateWithBasicOrdersAsync();
            var repository = new OrderRepository(context);

            //Act
            var result = await repository.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var order = result.First();

            order.User.Username.Should().Be("testuser");
            order.Status.Should().Be(OrderStatus.Draft);
        }
        [Fact]
        public async Task GetOne_ShouldReturnOrderWithUser_WhenOrderExists()
        {
            //Arrange
            var context = await TestDbContextFactory.CreateEmptyContextAsync(nameof(GetAll_ShouldReturnAllOrders));
            var repository = new OrderRepository(context);
            var user = new User
            {
                Id = 1,
                Username = "Usuario Test",
                Email = "usuario@test.com",
                Password = "123456"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var order = new Order
            {
                Status = OrderStatus.Placed,
                IsExpress = false,
                CreatedAt = DateTime.UtcNow,
                UserId = 1, // ya existe en el contexto
                Items = new List<OrderItem>() // sin ítems para este test
            };
            var createdOrder = await repository.Create(order);
            await context.SaveChangesAsync();
            //Act


            var result = await repository.GetOne(createdOrder.Id);

            //Assert
            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(createdOrder.Id);
            result.User.Should().NotBeNull();
            result.User.Username.Should().Be("Usuario Test");
        }
        [Fact]
        public async Task GetByUser_ShouldReturnOrdersWithDetails_WhenUserExists()
        {
            //Arrange
            var context = await TestDbContextFactory.CreateWithBasicOrdersAsync();
            var repository = new OrderRepository(context);
            //Act
            var result = await repository.GetByUser(1);
            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            var retrievedOrder = result.First();
            retrievedOrder.User.Username.Should().Be("testuser");
            retrievedOrder.Items.Should().HaveCount(2);
            retrievedOrder.Items.First().Product.Name.Should().Be("Auriculares");
            retrievedOrder.Items.Select(i => i.Product.Name).Should().Contain("Teclado");


        }
        [Fact]
        public async Task Update_ShouldModifyOrderDetails_WhenOrderExists()
        {
            //Arrange
            var context = await TestDbContextFactory.CreateWithBasicOrdersAsync();
            var repository = new OrderRepository(context);

            var existingOrder = context.Orders.First();
            var newStatus = OrderStatus.Shipped;
            var newExpressFlag = true;

            existingOrder.Status = newStatus;
            existingOrder.IsExpress = newExpressFlag;
            //Act
            var updateOrder = await repository.Update(existingOrder);
            //Assert
            updateOrder.Should().NotBeNull();
            updateOrder.Status.Should().Be(newStatus);
            updateOrder.IsExpress.Should().Be(newExpressFlag);

            var orderInDb = await context.Orders.FindAsync(existingOrder.Id);
            orderInDb!.Status.Should().Be(newStatus);
            orderInDb!.IsExpress.Should().Be(newExpressFlag);
        }
        [Fact]
        public async Task GetByStatus_ShouldReturnOrdersWithGivenStatusAndIncludeDetails()
        {
            //Arrange
            var context = await TestDbContextFactory.CreateWithBasicOrdersAsync();
            var repository = new OrderRepository(context);
            var expectedStatus = OrderStatus.Draft;

            //Act
            var result = await repository.GetByStatus(expectedStatus);
            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var order = result.First();
            order.Status.Should().Be(expectedStatus);
            order.Items.Should().NotBeEmpty();
            order.Items.First().Product.Should().NotBeNull();
        }
        [Fact]
        public async Task ChangeStatus_ShouldUpdateOrderStatus_WhenOrderExists()
        {
            //Arrange
            var context = await TestDbContextFactory.CreateEmptyContextAsync(nameof(ChangeStatus_ShouldUpdateOrderStatus_WhenOrderExists));
            var repository = new OrderRepository(context);
            var newStatus = OrderStatus.Shipped;
            var user = new User
            {
                Id = 1,
                Username = "Test User",
                Email = "user@test.com",
                Password = "123456"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var order = new Order
            {
                Status = OrderStatus.Draft,
                IsExpress = false,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                Items = new List<OrderItem>()
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            // Act

            await repository.ChangeStatus(order.Id, newStatus);
            // Assert
            var updatedOrder = await context.Orders.FindAsync(order.Id);
            updatedOrder.Should().NotBeNull();
            updatedOrder!.Status.Should().Be(newStatus);
        }
        [Fact]
        public async Task Delete_ShouldRemoveOrderFromDatabase_WhenOrderExists()
        {
            // Arrange
            var context = await TestDbContextFactory.CreateEmptyContextAsync(nameof(Delete_ShouldRemoveOrderFromDatabase_WhenOrderExists));
            var repository = new OrderRepository(context);

            var user = new User
            {
                Id = 1,
                Username = "Usuario Test",
                Email = "usuario@test.com",
                Password = "123456"
            };

            var order = new Order
            {
                Status = OrderStatus.Placed,
                IsExpress = false,
                CreatedAt = DateTime.UtcNow,
                UserId = 1
            };

            context.Users.Add(user);
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(order);

            // Assert
            result.Should().BeTrue();

            var deletedOrder = await context.Orders.FindAsync(order.Id);
            deletedOrder.Should().BeNull();
        }
    }
}
