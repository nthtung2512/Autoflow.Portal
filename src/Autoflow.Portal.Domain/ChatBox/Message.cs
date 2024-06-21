using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    // The table will have columns: Id, Content, ConversationId (FK to cid), SendUserId (FK to uid), and ReceiveUserId (FK to uid).
    public class Message : Entity<Guid>
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        public Guid SendUserId { get; set; }
        public User Sender { get; set; }

        public Guid ReceiveUserId { get; set; }
        public User Receiver { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public Message() { }

        public Message(string content, Guid sendUserId, Guid receiveUserId, Guid conversationId)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            SendUserId = sendUserId;
            ReceiveUserId = receiveUserId;
            ConversationId = conversationId;
        }
    }
}
