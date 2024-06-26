using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Infrastructure.Repositories
{
    public class MessageRepository(PortalDbContext context) : ChatBoxRepository(context), IMessageRepository
    {
        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(Guid messageId)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<Message>> GetMessagesForConversationAsync(Guid conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .ToListAsync();
        }

        public async Task AddMessageAsync(Message message)
        {
            Console.WriteLine("Adding message to database", message);
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(Guid messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}
