using Autoflow.Portal.Auth.Configurations;
using Autoflow.Portal.Base.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace Autoflow.Portal.Auth.EventHandlers
{
    internal class StoreRefreshTokenInCookieHandler
        : IOpenIddictServerHandler<ApplyTokenResponseContext>
    {
        private readonly CookieOptions cookieOptions;

        public StoreRefreshTokenInCookieHandler(
            IOptions<AuthenticationEndpointConfiguration> configuration
        )
        {
            cookieOptions = new()
            {
                Path = configuration.Value.ServerPath.RemovePostFix("/") + "/connect",
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.Now.Add(PortalAuthConst.RefreshTokenLifetime)
            };
        }

        public ValueTask HandleAsync(ApplyTokenResponseContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var request = context.Request;
            var response = context.Response;

            if (
                request != null
                && (request.IsRefreshTokenGrantType() || request.IsAuthorizationCodeGrantType())
            )
            {
                var aspResponse =
                    context.Transaction.GetHttpRequest()?.HttpContext.Response
                    ?? throw new InvalidOperationException("This is not Asp.Net response");

                if (response.RefreshToken != null)
                {
                    aspResponse.Cookies.Append(
                        TokenConst.RefreshToken,
                        response.RefreshToken,
                        cookieOptions
                    );
                    response.RemoveParameter(TokenConst.RefreshToken);
                }
            }

            return default;
        }
    }
}
