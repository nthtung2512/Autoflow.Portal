using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Domain.Organization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Autoflow.Portal.HttpApi.Controllers
{
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController(IOrganizationRepository organizationRepository) : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;

        [HttpPost]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        public async Task<IActionResult> AddOrganization([FromBody] Organization organization)
        {
            if (organization == null)
            {
                return BadRequest("Organization is null.");
            }

            await _organizationRepository.AddOrganizationAsync(organization);
            return CreatedAtAction(nameof(GetOrganizationById), new { id = organization.Id }, organization);
        }

        [HttpPost("messages")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "UserOnly")]
        public async Task<IActionResult> AddOrganizationMessage([FromBody] string message)
        {
            var organizationId = User.FindFirst("organization")?.Value;
            await _organizationRepository.AddOrganizationMessageAsync(new Guid(organizationId), message);
            return NoContent();
        }

        [HttpGet("admin")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var organizations = await _organizationRepository.GetAllOrganizationsAsync();
            return Ok(organizations);
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "UserOnly")]
        public async Task<IActionResult> GetOrganizationById()
        {
            var organizationId = User.FindFirst("organization")?.Value;
            var organization = await _organizationRepository.GetOrganizationByIdAsync(new Guid(organizationId));
            if (organization == null)
            {
                return NotFound();
            }

            return Ok(organization.OrganizationName);
        }

        [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = "UserOnly")]
        [HttpGet("messages")]
        public async Task<IActionResult> GetOrganizationMessages()
        {
            var organizationId = User.FindFirst("organization")?.Value;
            var messages = await _organizationRepository.GetOrganizationMessagesAsync(new Guid(organizationId));
            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }
    }
}
