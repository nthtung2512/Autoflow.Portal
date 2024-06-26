using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    public class Message(Guid id) : Entity<Guid>(id)
    {
        [StringLength(255, MinimumLength = 1)]
        public required string Content { get; set; }

        public Guid SendUserId { get; set; }
        public User? Sender { get; set; }

        public Guid ReceiveUserId { get; set; }
        public User? Receiver { get; set; }

        public Guid ConversationId { get; set; }
        public Conversation? Conversation { get; set; }
    }
}
