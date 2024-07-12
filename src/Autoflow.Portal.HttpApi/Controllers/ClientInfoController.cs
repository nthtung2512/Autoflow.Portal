using Autoflow.Portal.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientInfoController : ControllerBase
    {
        private readonly IClientInfoRepository _clientInfoRepository;

        public ClientInfoController(IClientInfoRepository clientInfoRepository)
        {
            _clientInfoRepository = clientInfoRepository;
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetClientInfo()
        {
            var clientId = User.GetClaim(Claims.Subject);
            var clientInfo = await _clientInfoRepository.GetClientInfoByIdAsync(clientId);
            if (clientInfo == null)
            {
                return NotFound();
            }

            return Ok(clientInfo);
        }
    }
}
