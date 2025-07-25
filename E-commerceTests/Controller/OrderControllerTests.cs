using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.application.Interfaces;
using E_commerce.Controllers;
using E_commerce.Domain.Enums;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;


namespace E_commerceTests.Controller
{
    public class OrderControllerTests
    {
        private readonly IOrderService _orderService;
        private readonly OrdersController _controller;
        public OrderControllerTests()
        {
            _orderService = A.Fake<IOrderService>();
            _controller = new OrdersController(_orderService);
        }
        [Fact]
        public async Task Create_ShouldReturnCreated_WhenDtoIsValid()
        {
            //Arrange
            var orderDto = new CreateOrderDto
            {
                UserId = 1,
                IsExpress = true,
                Items = new List<OrderItemRequest>
                    {
                        new OrderItemRequest { ProductId = 1, Quantity = 2 }
                    }
            };
            var validator = A.Fake<IValidator<CreateOrderDto>>();
            var validationResult = new FluentValidation.Results.ValidationResult(); // Sin errores => válido
            A.CallTo(() => validator.ValidateAsync(orderDto, default)).Returns(validationResult);
            var createdOrder = new OrderDto
            {
                Id = 123,
                UserId = orderDto.UserId,
                Status = OrderStatus.Draft,
                Items = new List<OrderItemDto>()
            };
            var orderService = A.Fake<IOrderService>();
            A.CallTo(() => orderService.Create(orderDto)).Returns(createdOrder);

            var controller = new OrdersController(orderService);

            // Act
            var result = await controller.Create(orderDto, validator);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.ActionName.Should().Be(nameof(OrdersController.GetByUser));
            createdResult.Value.Should().BeEquivalentTo(createdOrder);
        }
        [Fact]
        public async Task GetByUser_ShouldReturnOrders_WhenUserHasOrders()
        {
            //Arrange
            var userId = 5;
            var expectedOrders = new List<OrderDto>
            {
                new OrderDto { Id = 1, UserId = userId, Status = OrderStatus.Draft },
                new OrderDto { Id = 2, UserId = userId, Status = OrderStatus.Placed },
            };
            var orderService = A.Fake<IOrderService>();
            A.CallTo(() => orderService.GetByUser(userId))
                .Returns(expectedOrders);
            var controller = new OrdersController(orderService);
            //Act
            var result = await controller.GetByUser(userId);
            //Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedOrders);

        }
        [Fact]
        public async Task GetByStatus_ShouldReturnOrders_WhenOrdersWithStatusExist()
        {
            //Arrange
            var status = OrderStatus.Placed;
            var expectedOrders = new List<OrderDto>
                {
                    new OrderDto { Id = 10, Status = status, UserId = 1 },
                    new OrderDto { Id = 11, Status = status, UserId = 2 }
                };
            var fakeService = A.Fake<IOrderService>();
            A.CallTo(() => fakeService.GetByStatus(status))
                .Returns(expectedOrders);
            var controller = new OrdersController(fakeService);
            // Act
            var result = await controller.GetByStatus(status);
            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedOrders);
        }
        [Fact]
        public async Task GetByStatus_ShouldReturnEmptyList_WhenNoOrdersWithStatusExist()
        {
            // Arrange
            var status = OrderStatus.Placed;

            var fakeService = A.Fake<IOrderService>();
            A.CallTo(() => fakeService.GetByStatus(status))
                .Returns(new List<OrderDto>());

            var controller = new OrdersController(fakeService);

            // Act
            var result = await controller.GetByStatus(status);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(new List<OrderDto>());
        }
        [Fact]
        public async Task ChangeStatus_ShouldReturnNoContent_WhenValidationPasses()
        {
            //Arrange 
            //var fakeService = A.Fake<IOrderService>();
            var fakeValidator = A.Fake<IValidator<ChangeStatusDto>>();

            var orderId = 123;
            var dto = new ChangeStatusDto { Status = OrderStatus.Placed };

            var validationResult = new FluentValidation.Results.ValidationResult(); // isValid = true
            A.CallTo(() => fakeValidator.ValidateAsync(dto, default))
                .Returns(Task.FromResult(validationResult));
            // Act
            var result = await _controller.ChangeStatus(orderId, dto, fakeValidator);
            // Assert
            A.CallTo(() => _orderService.ChangeStatus(orderId, dto.Status)).MustHaveHappenedOnceExactly();
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
