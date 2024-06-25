using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.ChatBox;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Implement the methods for CRUP API for User, Message, and Conversation
namespace Autoflow.Portal.Infrastructure.Repositories
{
    public class ChatBoxRepository : IChatBoxRepository, IDisposable
    {
        private readonly PortalDbContext _context;

        public ChatBoxRepository(PortalDbContext context)
        {
            _context = context;
        }

        // User API

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            // If user not found -> return null -> handle in front end -> can ignore warning
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // Message API

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(Guid messageId)
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

        // Conversation API

        public async Task<Conversation> GetConversationByIdAsync(Guid conversationId)
        {
            return await _context.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<IEnumerable<Conversation>> GetAllConversationsAsync()
        {
            return await _context.Conversations.ToListAsync();
        }

        public async Task AddConversationAsync(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateConversationAsync(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
        }

        // UserConversation Map API
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

        // Additional methods as needed for your application
        #region dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) _context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
