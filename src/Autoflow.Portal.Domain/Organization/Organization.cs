using Autoflow.Portal.Base.EFCore;

namespace Autoflow.Portal.Domain.Organization
{
    public class Organization(Guid id) : Entity<Guid>(id)
    {
        public required string OrganizationName { get; set; } = string.Empty;
        public List<string> OrganizationMessages { get; set; } = [];
    }
}
