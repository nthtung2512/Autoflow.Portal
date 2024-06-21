using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.ChatBox
{
    public class Message : Entity<Guid>
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;

        public User? SendUser { get; set; }
        public User? ReceiveUser { get; set; }
    }
}
