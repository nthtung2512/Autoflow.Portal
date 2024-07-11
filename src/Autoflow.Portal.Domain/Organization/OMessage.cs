using Autoflow.Portal.Base.EFCore;
using System.ComponentModel.DataAnnotations;

namespace Autoflow.Portal.Domain.Organization
{
    public class OMessage(Guid id) : Entity<Guid>(id)
    {
        [StringLength(255, MinimumLength = 1)]
        public required string Content { get; set; }
        public string SenderClientId { get; set; } = string.Empty;
    }
}
