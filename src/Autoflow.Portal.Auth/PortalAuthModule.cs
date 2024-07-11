using Autoflow.Portal.Auth.Configurations;
using Autoflow.Portal.Base;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autoflow.Portal.Auth
{
    public class PortalAuthModule : Module
    {
        public override void ConfigureService(IHostApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.Configure<AuthenticationEndPoint>(
                configuration.GetSection("ClientConfiguration:AuthenticationEndpoint")
            );

            builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                       .UseDbContext<PortalDbContext>();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options.SetAuthorizationEndpointUris("connect/authorize")
                     .SetLogoutEndpointUris("connect/logout")
                     .SetTokenEndpointUris("connect/token")
                     .SetUserinfoEndpointUris("connect/userinfo");


                // Enable the client credentials flow.
                options.AllowClientCredentialsFlow();

                //    options.AddEncryptionKey(new SymmetricSecurityKey(
                //Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableLogoutEndpointPassthrough()
                .EnableTokenEndpointPassthrough()
                .EnableUserinfoEndpointPassthrough();
            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy =>
                policy.RequireRole("User"));
            });

            builder.Services.AddControllers();

            builder.Services.AddHostedService<Worker>();

        }
    }
}
