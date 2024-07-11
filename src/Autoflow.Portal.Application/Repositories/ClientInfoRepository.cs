using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.Shared;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Autoflow.Portal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.Application.Repositories
{
    public class ClientInfoRepository(PortalDbContext context) : ChatBoxRepository(context), IClientInfoRepository
    {
        public async Task<ClientInfo?> GetClientInfoByIdAsync(string oUserId)
        {
            return await _context.ClientInfos.FirstOrDefaultAsync(c => c.ClientId == oUserId);
        }
    }
}
