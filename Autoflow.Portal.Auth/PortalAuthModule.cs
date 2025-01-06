using Autoflow.Portal.Auth.Configurations;
using Autoflow.Portal.Auth.EventHandlers;
using Autoflow.Portal.Base;
using Autoflow.Portal.Base.Extensions;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;

namespace Autoflow.Portal.Auth
{
    public class PortalAuthModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            services.Configure<AuthenticationEndpointConfiguration>(
                configuration.GetSection(AuthenticationEndpointConfiguration.ConfigurationSection)
            );

            services
                .AddOpenIddict()
                .AddCore(options =>
                    options.UseEntityFrameworkCore().UseDbContext<PortalDbContext>()
                )
                .AddServer(options =>
                {
                    options
                        .SetAuthorizationEndpointUris(PortalAuthConst.AuthorizeEndpoint)
                        .SetTokenEndpointUris(PortalAuthConst.TokenEndpoint)
                        .SetUserinfoEndpointUris(PortalAuthConst.UserInfoEndpoint)
                        .SetLogoutEndpointUris(PortalAuthConst.LogoutEndpoint);

                    options
                        .AllowAuthorizationCodeFlow()
                        .AllowRefreshTokenFlow()
                        .RequireProofKeyForCodeExchange();

                    options
                        .SetAuthorizationCodeLifetime(PortalAuthConst.AuthorizationCodeLifetime)
                        .SetAccessTokenLifetime(PortalAuthConst.AccessTokenLifetime)
                        .SetRefreshTokenLifetime(PortalAuthConst.RefreshTokenLifetime);

#if DEBUG
                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();
#else
                    var encryptionCert = File.ReadAllBytes("encryption-certificate.pfx");
                    var signingCert = File.ReadAllBytes("signing-certificate.pfx");

                    options
                        .AddEncryptionCertificate(new X509Certificate2(encryptionCert))
                        .AddSigningCertificate(new X509Certificate2(signingCert));
#endif

                    options.UseTokenEndpointEventHandler();

                    options
                        .UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableLogoutEndpointPassthrough();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseSystemNetHttp();
                    options.UseAspNetCore();
                });

            services
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(
                            OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme
                        )
                        .Build();
                })
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddHostedService<Worker>();
        }
    }
}
