namespace Autoflow.Portal.Domain.ChatBox
{
    // Has column: UserId (FK to uid), ConversationId (FK to cid)
    public class UserConversationMap(Guid userId, Guid conversationId)
    {
        public Guid UserId { get; set; } = userId;
        public User? User { get; set; }

        public Guid ConversationId { get; set; } = conversationId;
        public Conversation? Conversation { get; set; }
    }
}
