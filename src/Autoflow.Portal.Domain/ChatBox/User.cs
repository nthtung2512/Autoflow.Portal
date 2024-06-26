using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    // The table will have columns: Id, Username, and Password. Referenced key to Message and UserConversationMap.
    public class User(Guid id) : Entity<Guid>(id)
    {
        // Properties
        [StringLength(32, MinimumLength = 4)]
        public required string Username { get; set; } = string.Empty;

        [StringLength(16, MinimumLength = 4)]
        public required string Password { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Message> SentMessages { get; set; } = [];
        public ICollection<Message> ReceivedMessages { get; set; } = [];
        public ICollection<UserConversationMap> UserConversations { get; set; } = [];
    }
}
