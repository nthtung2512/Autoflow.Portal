using Autoflow.Portal.Domain.ChatBox;

namespace Autoflow.Portal.Application.Contracts.Repositories
{
    public interface IUserConversationMapRepository
    {
        Task<IEnumerable<UserConversationMap>> GetAllUserConversationMaps();
        Task<IEnumerable<UserConversationMap>> GetMapByUserId(Guid userId);
        Task<IEnumerable<UserConversationMap>> GetMapByConversationId(Guid conversationId);
        Task AddUserConversationMapAsync(UserConversationMap userConversationMap);
    }
}
