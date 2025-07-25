using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.Domain.Enums;

namespace E_commerce.application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> Create(CreateOrderDto createOrderDto);
        Task<IEnumerable<OrderDto>> GetByUser(int userId);
        Task ChangeStatus(int orderId, OrderStatus newStatus);
        Task<IEnumerable<OrderDto>> GetByStatus(OrderStatus status);
    }
}
