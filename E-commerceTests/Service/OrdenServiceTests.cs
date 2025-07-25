using AutoMapper;
using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.application.Services;
using E_commerce.Domain.Enums;
using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;
using FakeItEasy;
using FluentAssertions;


namespace E_commerceTests.Service
{
    public class OrdenServiceTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderQueue _orderQueue;
        private readonly IMapper _mapper;
        private readonly OrderService _orderService;
        public OrdenServiceTests()
        {
            _orderRepository = A.Fake<IOrderRepository>();
            _productRepository = A.Fake<IProductRepository>();
            _orderQueue = A.Fake<IOrderQueue>();
            _mapper = A.Fake<IMapper>();

            _orderService = new OrderService(_orderRepository, _productRepository, _orderQueue, _mapper);
        }
        [Fact]
        public async Task Create_ShouldCreateOrder_WhenDataIsValid()
        {
            //Arrange
            var createDto = new CreateOrderDto
            {
                UserId = 1,
                IsExpress = false,
                Items = new List<OrderItemRequest>
                {
                    new OrderItemRequest { ProductId = 10, Quantity = 2 }
                }
            };

            var product = new Product
            {
                Id = 10,
                Name = "Teclado",
                Stock = 5,
                Price = 100m
            };
            A.CallTo(() => _productRepository.GetById(10))
                .Returns(product);
            Order caputuredOrder = null!;
            A.CallTo(() => _orderRepository.Create(A<Order>.Ignored))
                .Invokes((Order o) => caputuredOrder = o)
                .ReturnsLazily(() => Task.FromResult(caputuredOrder));

            var expectedDto = new OrderDto { Id = 999 };
            A.CallTo(() => _mapper.Map<OrderDto>(A<Order>.Ignored)).Returns(expectedDto);

            //Act
            var result = await _orderService.Create(createDto);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(999);

            A.CallTo(() => _productRepository.GetById(10)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepository.Create(A<Order>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<OrderDto>(A<Order>._)).MustHaveHappenedOnceExactly();


            caputuredOrder.Should().NotBeNull();
            caputuredOrder.UserId.Should().Be(1);
            caputuredOrder.Items.Should().HaveCount(1);
            caputuredOrder.Status.Should().Be(OrderStatus.Draft);
        }
        [Fact]
        public async Task GetByStatus_ShouldReturnMappedDtos_WhenOrdersExist()
        {
            //Arrange
            var status = OrderStatus.Placed;

            var fakeOrders = new List<Order>
    {
                new Order
                {
                    Id = 1,
                    Status = status,
                    CreatedAt = DateTime.UtcNow,
                    IsExpress = true
                },
                new Order
                {
                    Id = 2,
                    Status = status,
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    IsExpress = false
                }
            };
            var expectedDtos = new List<OrderDto>
                    {
                        new OrderDto { Id = 1, Status = status },
                        new OrderDto { Id = 2, Status = status }
                    };
            A.CallTo(() => _orderRepository.GetByStatus(status))
                .Returns(Task.FromResult<IEnumerable<Order>>(fakeOrders));

            A.CallTo(() => _mapper.Map<IEnumerable<OrderDto>>(fakeOrders))
                .Returns(expectedDtos);

            ///Act
            var result = await _orderService.GetByStatus(status);

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Status.Should().Be(status);

            A.CallTo(() => _orderRepository.GetByStatus(status)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<IEnumerable<OrderDto>>(fakeOrders)).MustHaveHappenedOnceExactly();


        }
        [Fact]
        public async Task GetByUser_ShouldReturnMappedDtos_WhenOrdersExist()
        {
            // Arrange
            var userId = 10;

            var fakeOrders = new List<Order>
    {
        new Order { Id = 1, UserId = userId, Status = OrderStatus.Placed },
        new Order { Id = 2, UserId = userId, Status = OrderStatus.Shipped }
    };

            var expectedDtos = new List<OrderDto>
    {
        new OrderDto { Id = 1, Status = OrderStatus.Placed },
        new OrderDto { Id = 2, Status = OrderStatus.Shipped}
    };

            A.CallTo(() => _orderRepository.GetByUser(userId))
                .Returns(Task.FromResult<IEnumerable<Order>>(fakeOrders));

            A.CallTo(() => _mapper.Map<IEnumerable<OrderDto>>(fakeOrders))
                .Returns(expectedDtos);

            // Act
            var result = await _orderService.GetByUser(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Status.Should().Be(OrderStatus.Shipped);

            A.CallTo(() => _orderRepository.GetByUser(userId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<IEnumerable<OrderDto>>(fakeOrders)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task ChangeStatus_ShouldUpdateStatusAndEnqueue_WhenStatusIsPlaced()
        {
            // Arrange
            var orderId = 1;
            var newStatus = OrderStatus.Placed;


            var order = new Order
            {
                Id = orderId,
                Status = OrderStatus.Draft,
                IsExpress = true,
                UserId = 10,
                Items = new List<OrderItem>()
            };


            A.CallTo(() => _orderRepository.Exists(orderId)).Returns(true);
            A.CallTo(() => _orderRepository.GetOne(orderId)).Returns(order);

            Order? capturedOrder = null;
            A.CallTo(() => _orderRepository.Update(A<Order>._))
                .Invokes((Order o) => capturedOrder = o)

  .ReturnsLazily(() => Task.FromResult(capturedOrder!));

            // Act
            await _orderService.ChangeStatus(orderId, newStatus);

            // Assert


            A.CallTo(() => _orderRepository.Exists(orderId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepository.GetOne(orderId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _orderRepository.Update(A<Order>._)).MustHaveHappenedOnceExactly();

            A.CallTo(() => _orderQueue.Enqueue(
        A<Order>.That.Matches(o =>
            o.Id == orderId && o.Status == newStatus)))
     .MustHaveHappenedOnceExactly();
        }

    }
}
