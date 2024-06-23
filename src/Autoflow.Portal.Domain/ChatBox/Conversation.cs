using Autoflow.Portal.Base.EFCore;

namespace Autoflow.Portal.Domain.ChatBox
{
    // Have column: Id
    public class Conversation : Entity<Guid>
    {
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<UserConversationMap> UserConversations { get; set; } = new List<UserConversationMap>();

        public Conversation(Guid id) : base(id)
        {

        }
    }
}
