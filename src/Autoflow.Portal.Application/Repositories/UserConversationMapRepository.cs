using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Infrastructure.Repositories
{
    public class UserConversationMapRepository(PortalDbContext context) : ChatBoxRepository(context), IUserConversationMapRepository
    {
        public async Task<IEnumerable<UserConversationMap>> GetAllUserConversationMaps()
        {
            return await _context.UserConversationMaps.ToListAsync();
        }

        public async Task<IEnumerable<UserConversationMap>> GetMapByUserId(Guid userId)
        {
            return await _context.UserConversationMaps
                .Where(ucm => ucm.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserConversationMap>> GetMapByConversationId(Guid conversationId)
        {
            return await _context.UserConversationMaps
                .Where(ucm => ucm.ConversationId == conversationId)
                .ToListAsync();
        }

        public async Task AddUserConversationMapAsync(UserConversationMap userConversationMap)
        {
            _context.UserConversationMaps.Add(userConversationMap);
            await _context.SaveChangesAsync();
        }
    }
}
