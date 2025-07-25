using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;

namespace E_commerce.Domain.Common
{
    public class OrderQueue : IOrderQueue
    {
        // prioridad: 0 = exprés (más alta), 1 = normal
        private readonly PriorityQueue<Order, int> _pq = new();

        public void Enqueue(Order order)
        {
            int prio = order.IsExpress ? 0 : 1;
            _pq.Enqueue(order, prio);
        }

        public Order? Dequeue()
        {
            return _pq.TryDequeue(out var order, out _)
                ? order
                : null;
        }
    }
}
