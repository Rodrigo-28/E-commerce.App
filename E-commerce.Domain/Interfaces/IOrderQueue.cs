using E_commerce.Domain.Models;

namespace E_commerce.Domain.Interfaces
{
    public interface IOrderQueue
    {
        void Enqueue(Order order);
        Order? Dequeue();
    }
}
