using Microsoft.AspNetCore;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace Autoflow.Portal.Auth.EventHandlers
{
    internal class CookieAuthenticationHandler
        : IOpenIddictServerHandler<OpenIddictServerEvents.ExtractTokenRequestContext>
    {
        public ValueTask HandleAsync(ExtractTokenRequestContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            var request = context.Request;
            if (request == null)
                return default;

            if (request.IsRefreshTokenGrantType())
            {
                var aspRequest =
                    context.Transaction.GetHttpRequest()
                    ?? throw new InvalidOperationException("This is not Asp.Net response");

                aspRequest.Cookies.TryGetValue(TokenConst.RefreshToken, out var refreshToken);

                request.RefreshToken = refreshToken;
            }

            return default;
        }
    }
}
