using Autoflow.Portal.Domain.ChatBox;

namespace Autoflow.Portal.Application.Contracts.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message?> GetMessageByIdAsync(Guid messageId);
        Task<IEnumerable<Message>> GetMessagesForConversationAsync(Guid conversationId);
        Task AddMessageAsync(Message message);
        Task DeleteMessageAsync(Guid messageId);
    }
}
