using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Infrastructure.Repositories
{
    public class ConversationRepository(PortalDbContext context) : ChatBoxRepository(context), IConversationRepository
    {
        public async Task<Conversation?> GetConversationByIdAsync(Guid conversationId)
        {
            return await _context.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<IEnumerable<Conversation>> GetAllConversationsAsync()
        {
            return await context.Conversations.ToListAsync();
        }

        public async Task AddConversationAsync(Conversation conversation)
        {
            context.Conversations.Add(conversation);
            await context.SaveChangesAsync();
        }

        public async Task UpdateConversationAsync(Conversation conversation)
        {
            context.Conversations.Update(conversation);
            await context.SaveChangesAsync();
        }
    }
}
