using Autoflow.Portal.Domain.Shared;

namespace Autoflow.Portal.Application.Contracts.Repositories
{
    public interface IClientInfoRepository
    {
        Task<ClientInfo?> GetClientInfoByIdAsync(string oUserId);
    }
}
