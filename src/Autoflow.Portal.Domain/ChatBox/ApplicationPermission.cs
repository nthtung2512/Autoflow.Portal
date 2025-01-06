using Autoflow.Portal.Base.EFCore;
using Autoflow.Portal.Domain.Shared.Roles;

namespace Autoflow.Portal.Domain.ChatBox
{
    public class ApplicationPermission : Entity<int>
    {
        public required User User { get; init; }
        public required UserRole Role { get; set; }
    }
}
