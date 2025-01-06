using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Application.Repositories;
using Autoflow.Portal.Auth;
using Autoflow.Portal.Domain.ChatBox;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Immutable;
using System.Net.Mime;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Autoflow.Portal.HttpApi.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IUserRepository _userRepository;
        private readonly PermissionRepository _permissionRepository;

        public AuthorizationController(
            IUserRepository userRepository,
            IOpenIddictScopeManager scopeManager,
            PermissionRepository permissionRepository
        )
        {
            _userRepository = userRepository;
            _scopeManager = scopeManager;
            _permissionRepository = permissionRepository;
        }

        [HttpPost(PortalAuthConst.AuthorizeEndpoint)]
        [HttpGet(PortalAuthConst.AuthorizeEndpoint)]
        public async Task<IActionResult> AuthorizeWithEmailPasswordAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            if (request == null)
                return Unauthorized();

            var username = request.GetParameter(PortalAuthConst.Username)?.ToString();
            var password = request.GetParameter(PortalAuthConst.Password)?.ToString();

            if (username == null || password == null)
                return Unauthorized();

            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || user.Password != password)
            {
                return Challenge(
                    authenticationSchemes:
                    [
                        OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
                    ],
                    properties: new AuthenticationProperties(
                        new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                                Errors.InvalidRequest,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "Username or password is not valid"
                        }
                    )
                );
            }

            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role
            );

            await SetClaimsAsync(identity, user);
            identity.SetScopes(request!.GetScopes());

            identity.SetResources(
                _scopeManager
                    .ListResourcesAsync(identity.GetScopes())
                    .ToBlockingEnumerable()
                    .ToList()
            );

            // Allow all claims to be added in the access tokens.
            identity.SetDestinations(claim => [Destinations.AccessToken]);

            return SignIn(
                new ClaimsPrincipal(identity),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
        }

        [HttpGet(PortalAuthConst.TokenEndpoint), HttpPost(PortalAuthConst.TokenEndpoint)]
        [IgnoreAntiforgeryToken]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ExchangeTokenAsync()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            if (request == null)
                return Unauthorized();

            if (!(request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType()))
                return Unauthorized("The specified grant type is not supported.");

            // Retrieve the claims principal stored in the authorization code/refresh token.
            var result = await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
            if (result == null)
                return Unauthorized("The token is no longer valid");

            // Retrieve the user profile corresponding to the authorization code/refresh token.
            var user = await _userRepository.GetUserByUsernameAsync(
                result.Principal!.GetClaim(Claims.Username)!
            );

            if (user is null)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(
                        new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                                Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "User data doesn't match"
                        }
                    )
                );
            }

            var identity = new ClaimsIdentity(
                result.Principal!.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role
            );

            // Override the user claims present in the principal in case they
            // changed since the authorization code/refresh token was issued.
            await SetClaimsAsync(identity, user);
            identity.SetDestinations(claim => [Destinations.AccessToken]);

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(
                new ClaimsPrincipal(identity),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            );
        }

        [Authorize]
        [HttpPost(PortalAuthConst.UserInfoEndpoint), HttpGet(PortalAuthConst.UserInfoEndpoint)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetUserInfoAsync()
        {
            var user = await _userRepository.GetUserByIdAsync(new Guid(User.GetClaim(Claims.Subject)!));

            if (user is null)
            {
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(
                        new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                                Errors.InvalidToken,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The specified access token is bound to an account that no longer exists."
                        }
                    )
                );
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                [Claims.Subject] = user.Id,
                [Claims.Name] = user.Username,
                [Claims.Role] = (await _permissionRepository.SelectAllUserRolesAsync(user.Id))
                    .Select(role => role.ToString())
                    .ToImmutableArray()
            };

            return Ok(claims);
        }

        [HttpGet(PortalAuthConst.LogoutEndpoint)]
        [HttpPost(PortalAuthConst.LogoutEndpoint)]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutPost()
        {
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties { RedirectUri = "/" }
            );
        }

        private async Task SetClaimsAsync(ClaimsIdentity identity, User user)
        {
            // Add the claims that will be persisted in the tokens.
            identity
                .SetClaim(Claims.Subject, user.Id.ToString())
                .SetClaim(Claims.Name, user.Username)
                .SetClaims(
                    Claims.Role,
                    (await _permissionRepository.SelectAllUserRolesAsync(user.Id))
                        .Select(role => role.ToString())
                        .ToImmutableArray()
                );
        }
    }
}
