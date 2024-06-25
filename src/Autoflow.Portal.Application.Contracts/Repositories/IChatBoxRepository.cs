using Autoflow.Portal.Domain.ChatBox;

namespace Autoflow.Portal.Application.Contracts.Repositories
{
    public interface IChatBoxRepository
    {
        // User API
        Task<User> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);

        // Message API
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message> GetMessageByIdAsync(Guid messageId);
        Task<IEnumerable<Message>> GetMessagesForConversationAsync(Guid conversationId);
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(Guid messageId);

        // Conversation API
        Task<Conversation> GetConversationByIdAsync(Guid conversationId);
        Task<IEnumerable<Conversation>> GetAllConversationsAsync();
        Task AddConversationAsync(Conversation conversation);
        Task UpdateConversationAsync(Conversation conversation);

        // UserConversation Map API
        Task<IEnumerable<UserConversationMap>> GetAllUserConversationMaps();
        Task<IEnumerable<UserConversationMap>> GetMapByUserId(Guid userId);
        Task<IEnumerable<UserConversationMap>> GetMapByConversationId(Guid conversationId);
        Task AddUserConversationMapAsync(UserConversationMap userConversationMap);



        // Additional methods as needed for your application
    }
}
