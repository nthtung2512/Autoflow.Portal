using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Autoflow.Portal.Auth.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IClientInfoRepository _clientInfoRepository;
        public AuthorizationController(IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager, IClientInfoRepository clientInfoRepository)
        {
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _clientInfoRepository = clientInfoRepository;
        }

        [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var application = await _clientInfoRepository.GetClientInfoByIdAsync(request.ClientId);
                if (application == null)
                {
                    throw new InvalidOperationException("The application details cannot be found in the database.");
                }

                // Create the claims-based identity that will be used by OpenIddict to generate tokens.
                var identity = new ClaimsIdentity(
                    authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                    nameType: Claims.Name,
                    roleType: Claims.Role);

                // Add the claims that will be persisted in the tokens (use the client_id as the subject identifier).
                identity.SetClaim(Claims.Subject, application.ClientId.ToString())
                        .SetClaim(Claims.Name, application.DisplayName)
                        .SetClaim("organization", application.OrganizationId.ToString())
                        .SetClaim(Claims.Role, application.Role);
                // Note: In the original OAuth 2.0 specification, the client credentials grant
                // doesn't return an identity token, which is an OpenID Connect concept.
                //
                // As a non-standardized extension, OpenIddict allows returning an id_token
                // to convey information about the client application when the "openid" scope
                // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
                // scope is not explicitly set, no identity token is returned to the client application.

                // Set the list of scopes granted to the client application in access_token.

                // Scope can be "read", "write", "openid", etc. Set to determine which scope user can perform. => Access restrictions.
                // Set the scopes according to the "User" or "Admin" role
                identity.SetScopes(request.GetScopes());

                // Sets the resources that the client application is allowed to access.
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

                // Note: by default, claims are NOT automatically included in the access and identity tokens.
                // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
                // whether they should be included in access tokens, in identity tokens or in both.

                identity.SetDestinations(GetDestinations);

                return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {

            return claim.Type switch
            {
                Claims.Name or Claims.Subject => [Destinations.AccessToken, Destinations.IdentityToken],

                // Allow the "name" claim to be stored in both the access and identity tokens
                // when the "profile" scope was granted (by calling principal.SetScopes(...)).
                //Claims.Name when claim.Subject.HasScope(Scopes.Profile)
                //    => [Destinations.AccessToken, Destinations.IdentityToken],

                _ => [Destinations.AccessToken],
            };
        }
    }

}
