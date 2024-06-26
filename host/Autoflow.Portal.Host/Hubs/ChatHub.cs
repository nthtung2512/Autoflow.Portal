using Autoflow.Portal.Domain.ChatBoxDTOs;
using Microsoft.AspNetCore.SignalR;

namespace Autoflow.Portal.Host.Hubs
{
    // The Hub class manages connections, groups, and messaging.
    public class ChatHub : Hub
    {
        // When SendMessage is called by client A, message is sent to all client 
        public async Task SendMessage(MessageDTO messageDTO)
        {
            //string name = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", messageDTO);
        }

        public async Task PostUserMessage(UserDTO userDTO)
        {
            await Clients.All.SendAsync("ReceivePostUserMessage", userDTO);
        }

        public async Task DeleteMessage(MessageDTO messageDTO)
        {
            await Clients.All.SendAsync("ReceiveDeleteMessage", messageDTO);
        }
    }
}

//using Autoflow.Portal.Domain.ChatBoxDTOs;
//using Autoflow.Portal.Host.ConnectionMapping;
//using Microsoft.AspNetCore.SignalR;

//namespace Autoflow.Portal.Host.Hubs
//{
//    public class ChatHub : Hub
//    {
//        // Dictionary to map user IDs to connection IDs
//        private readonly static ConnectionMapping<string> _connections =
//            new ConnectionMapping<string>();

//        // Override OnConnectedAsync to map user ID to connection ID
//        public override async Task OnConnectedAsync()
//        {
//            string name = Context.User.Identity.Name;

//            _connections.Add(name, Context.ConnectionId);
//            await base.OnConnectedAsync();
//        }

//        // Override OnDisconnectedAsync to remove user ID from the map
//        public override async Task OnDisconnectedAsync(Exception exception)
//        {
//            string name = Context.User.Identity.Name;

//            _connections.Remove(name, Context.ConnectionId);
//            await base.OnDisconnectedAsync(exception);
//        }

//        // SendMessage method to send a message to a specific user
//        public async Task SendMessage(MessageDTO messageDTO)
//        {
//            string name = Context.User.Identity.Name;
//            foreach (var connectionId in _connections.GetConnections(messageDTO.ReceiveUserId.ToString()))
//            {
//                await Clients.Client(connectionId).SendAsync("ReceiveMessage", messageDTO);
//            }
//        }
//    }
//}
