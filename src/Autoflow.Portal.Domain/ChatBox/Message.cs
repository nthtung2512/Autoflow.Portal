using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    public class Message : Entity<Guid>
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Content { get; set; }

        public Guid SendUserId { get; set; }
        public User Sender { get; set; }

        public Guid ReceiveUserId { get; set; }
        public User Receiver { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        // Constructors and additional logic as needed
        public Message(Guid id, string content, Guid sendUserId, Guid receiveUserId, Guid conversationId)
        : base(id)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            SendUserId = sendUserId;
            ReceiveUserId = receiveUserId;
            ConversationId = conversationId;
        }
    }
}
