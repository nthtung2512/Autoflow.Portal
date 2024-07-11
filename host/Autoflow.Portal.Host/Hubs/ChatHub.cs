//using Autoflow.Portal.Domain.ChatBoxDTOs;
//using Microsoft.AspNetCore.SignalR;
//using System.Security.Claims;

//namespace Autoflow.Portal.Host.Hubs
//{
//    // The Hub class manages connections, groups, and messaging.
//    public class ChatHub : Hub
//    {
//        // When SendMessage is called by client A, message is sent to all client 
//        public async Task SendMessage(MessageDTO messageDTO)
//        {
//            var userIdClaim = Context.User?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
//            string name = Context.User.Identity.Name;
//            await Clients.All.SendAsync("ReceiveMessage", messageDTO);
//        }

//        public async Task PostUserMessage(UserDTO userDTO)
//        {
//            await Clients.All.SendAsync("ReceivePostUserMessage", userDTO);
//        }

//        public async Task DeleteMessage(MessageDTO messageDTO)
//        {
//            await Clients.All.SendAsync("ReceiveDeleteMessage", messageDTO);
//        }
//    }
//}

using Autoflow.Portal.Domain.ChatBoxDTOs;
using Autoflow.Portal.Host.ConnectionMapping;
using Microsoft.AspNetCore.SignalR;

namespace Autoflow.Portal.Host.Hubs
{
    public class ChatHub : Hub
    {
        // Dictionary to map user IDs to connection IDs
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
            _connections.Add(conversationId, Context.ConnectionId);
        }

        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
            _connections.Remove(conversationId, Context.ConnectionId);
        }

        public async Task SendMessage(MessageDTO messageDTO)
        {
            var connections = _connections.GetConnections(messageDTO.ConversationId.ToString());
            if (connections.Any())
            {
                await Clients.Group(messageDTO.ConversationId.ToString()).SendAsync("ReceiveMessage", messageDTO);
            }
        }

        public async Task SendSimpleMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveSimpleMessage", message);
        }

        public async Task DeleteMessage(MessageDTO messageDTO)
        {
            var connections = _connections.GetConnections(messageDTO.ConversationId.ToString());
            if (connections.Any())
            {
                await Clients.Group(messageDTO.ConversationId.ToString()).SendAsync("ReceiveDeleteMessage", messageDTO);
            }
        }

        public async Task PostUserMessage(UserDTO userDTO)
        {
            await Clients.All.SendAsync("ReceivePostUserMessage", userDTO);
        }
    }
}
