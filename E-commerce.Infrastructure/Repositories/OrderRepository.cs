using E_commerce.Domain.Enums;
using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;
using E_commerce.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Order> Create(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOne(int id)
        {
            var order = await _context.Orders.
                Include(u => u.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }

        public async Task<IEnumerable<Order>> GetByUser(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order> Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;


        }

        public async Task<IEnumerable<Order>> GetByStatus(OrderStatus orderStatus)
        {
            return await _context.Orders
                .Where(o => o.Status == orderStatus)
                 .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                 .ToListAsync();
        }

        //public async Task<IEnumerable<Order>> GetByStatus(OrderStatus status)
        //{
        //    await _context.Orders
        //         .Where(o => o.Status == status)
        //         .ToListAsync();
        //}

        public async Task ChangeStatus(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            order.Status = newStatus;
            await _context.SaveChangesAsync();

        }


    }
}
