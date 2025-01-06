using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Server;

namespace Autoflow.Portal.Auth.EventHandlers
{
    internal static class OpenIddictServerBuilderExtensions
    {
        public static void UseTokenEndpointEventHandler(this OpenIddictServerBuilder options)
        {
            options.AddEventHandler<OpenIddictServerEvents.ApplyTokenResponseContext>(static conf =>
                conf.UseSingletonHandler<StoreRefreshTokenInCookieHandler>()
            );

            options.AddEventHandler<OpenIddictServerEvents.ExtractTokenRequestContext>(
                static conf => conf.UseSingletonHandler<CookieAuthenticationHandler>()
            );
        }
    }
}
