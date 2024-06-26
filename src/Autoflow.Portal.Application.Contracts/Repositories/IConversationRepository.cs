using Autoflow.Portal.Domain.ChatBox;

namespace Autoflow.Portal.Application.Contracts.Repositories
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetConversationByIdAsync(Guid conversationId);
        Task<IEnumerable<Conversation>> GetAllConversationsAsync();
        Task AddConversationAsync(Conversation conversation);
        Task UpdateConversationAsync(Conversation conversation);
    }
}
