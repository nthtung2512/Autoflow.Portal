using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.Organization;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Autoflow.Portal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Application.Repositories
{
    public class OrganizationRepository(PortalDbContext context) : ChatBoxRepository(context), IOrganizationRepository
    {
        public async Task AddOrganizationAsync(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
        }

        public async Task AddOrganizationMessageAsync(Guid organizationId, string message)
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);
            if (organization != null)
            {
                organization.OrganizationMessages.Add(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization?> GetOrganizationByIdAsync(Guid organizationId)
        {
            return await _context.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);
        }

        public async Task<List<string>> GetOrganizationMessagesAsync(Guid organizationId)
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);
            if (organization != null)
            {
                return organization.OrganizationMessages;
            }
            return null;

        }
    }
}
