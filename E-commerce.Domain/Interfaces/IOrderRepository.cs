using E_commerce.Domain.Enums;
using E_commerce.Domain.Models;

namespace E_commerce.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOne(int id);
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetByUser(int userId);
        Task<Order> Create(Order order);
        Task<Order> Update(Order order);
        Task<bool> Delete(Order order);
        Task<bool> Exists(int id);
        //Task<IEnumerable<Order>> GetByStatus(Order order);
        Task<IEnumerable<Order>> GetByStatus(OrderStatus status);
        Task ChangeStatus(int orderId, OrderStatus newStatus);


    }
}
