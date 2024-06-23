using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    // The table will have columns: Id, Username, and Password. Referenced key to Message and UserConversationMap.
    public class User : Entity<Guid>
    {
        // Properties
        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;

        public User(Guid id, string username, string password)
            : base(id)
        {
            Username = username;
            Password = password;
        }

        // Navigation Properties
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public ICollection<UserConversationMap> UserConversations { get; set; } = new List<UserConversationMap>();
    }
}
