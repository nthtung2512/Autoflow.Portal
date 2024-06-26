using Autoflow.Portal.Base.EFCore;

namespace Autoflow.Portal.Domain.ChatBox
{
    // Have column: Id
    public class Conversation(Guid id) : Entity<Guid>(id)
    {
        public ICollection<Message> Messages { get; set; } = [];
        public ICollection<UserConversationMap> UserConversations { get; set; } = [];
    }
}
