using Microsoft.AspNetCore.SignalR;

namespace Autoflow.Portal.Host.Hubs
{
    // The Hub class manages connections, groups, and messaging.
    public class ChatHub : Hub
    {
        // SendMessage method can be called by a connected client to send a message to all clients.
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
