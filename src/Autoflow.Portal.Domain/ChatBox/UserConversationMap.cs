namespace Autoflow.Portal.Domain.ChatBox
{
    // Has column: UserId (FK to uid), ConversationId (FK to cid)
    public class UserConversationMap
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }
    }
}
