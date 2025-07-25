using AutoMapper;
using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using E_commerce.Domain.Enums;
using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;

namespace E_commerce.application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderQueue _orderQueue;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IOrderQueue orderQueue, IMapper mapper)
        {
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._orderQueue = orderQueue;
            this._mapper = mapper;
        }

        public async Task ChangeStatus(int orderId, OrderStatus newStatus)
        {
            if (!await _orderRepository.Exists(orderId))
            {
                throw new NotFoundException($"Order {orderId} not found")
                {
                    ErrorCode = "007"
                };
            }

            var order = await _orderRepository.GetOne(orderId);
            order.Status = newStatus;


            var updated = await _orderRepository.Update(order);


            //Sólo encolar si pasamos a Placed
            if (newStatus == OrderStatus.Placed)
            {
                Console.WriteLine(
                    $"[Service] Enqueuing Order {updated.Id} | Express: {updated.IsExpress}");
                _orderQueue.Enqueue(updated);
            }
        }

        public async Task<OrderDto> Create(CreateOrderDto createOrderDto)
        {
            var items = new List<OrderItem>();

            foreach (var req in createOrderDto.Items)
            {
                var prod = await _productRepository.GetById(req.ProductId);
                if (prod == null)
                {
                    throw new NotFoundException($"Product {req.ProductId} not found")
                    {
                        ErrorCode = "055"
                    };

                }
                if (prod.Stock < req.Quantity)
                {
                    throw new BadRequestException(
                       $"Insufficient stock for product {req.ProductId}")
                    {
                        ErrorCode = "056"
                    };
                }
                items.Add(new OrderItem
                {
                    ProductId = req.ProductId,
                    Quantity = req.Quantity,
                    UnitPrice = prod.Price,
                });

            }

            // 3) Construir entidad Order en estado Draft (carrito)
            var order = new Order
            {
                UserId = createOrderDto.UserId,
                IsExpress = createOrderDto.IsExpress,
                Status = OrderStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                Items = items
            };

            var created = await _orderRepository.Create(order);


            return _mapper.Map<OrderDto>(created);
        }

        public async Task<IEnumerable<OrderDto>> GetByStatus(OrderStatus status)
        {
            var orders = await _orderRepository.GetByStatus(status);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<OrderDto>> GetByUser(int userId)
        {
            var orders = await _orderRepository.GetByUser(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }

}
