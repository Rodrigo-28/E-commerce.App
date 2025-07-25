using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace E_commerce.Helpers
{
    [Authorize(Policy = "AuthenticatedUser")]
    public class NotificationsHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            var userId = Context.UserIdentifier;
            Console.WriteLine($"🔑 Hub conectado: {userId}");

            await base.OnConnectedAsync();
        }
    }
}
