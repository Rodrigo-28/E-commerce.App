using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;
using E_commerce.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace E_commerce.WebAPI.HostedServices
{
    public class OrderProcessingService : BackgroundService
    {
        private readonly IOrderQueue _orderQueue;
        private readonly IHubContext<NotificationsHub> _hub;

        public OrderProcessingService(
             IOrderQueue orderQueue,
            IHubContext<NotificationsHub> hub)
        {
            _orderQueue = orderQueue;
            _hub = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                //vaciar toda la cola
                await ProcessAllAsync(stoppingToken);
            }
        }

        public async Task ProcessAllAsync(CancellationToken cancellationToken = default)
        {
            Order order;
            while ((order = _orderQueue.Dequeue()) != null)
            {
                Console.WriteLine($"[Manual] Dequeued {order.Id} | Express: {order.IsExpress}");
                await _hub.Clients.All
                          .SendAsync("OrderProcessed", new
                          {
                              Id = order.Id,
                              Status = order.Status.ToString()
                          }, cancellationToken);
            }
        }
    }
}
