using Autoflow.Portal.Domain.Organization;

namespace Autoflow.Portal.Application.Contracts.Repositories
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization?> GetOrganizationByIdAsync(Guid organizationId);
        Task AddOrganizationAsync(Organization organization);
        Task<List<string>> GetOrganizationMessagesAsync(Guid organizationId);
        Task AddOrganizationMessageAsync(Guid organizationId, string message);
    }
}
