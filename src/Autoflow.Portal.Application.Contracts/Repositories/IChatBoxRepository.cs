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
        Task<Message> GetMessageByIdAsync(Guid messageId);
        Task<IEnumerable<Message>> GetMessagesForConversationAsync(Guid conversationId);
        Task AddMessageAsync(Message message);

        // Conversation API
        Task<Conversation> GetConversationByIdAsync(Guid conversationId);
        Task<IEnumerable<Conversation>> GetAllConversationsAsync();
        Task AddConversationAsync(Conversation conversation);
        Task UpdateConversationAsync(Conversation conversation);

        // Additional methods as needed for your application
    }
}
