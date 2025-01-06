using Autoflow.Portal.Auth.Configurations;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Autoflow.Portal.Auth
{
    internal class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthenticationEndpointConfiguration _authClientConfiguration;

        public Worker(
            IServiceProvider serviceProvider,
            IOptions<AuthenticationEndpointConfiguration> clientConfiguration
        )
        {
            _serviceProvider = serviceProvider;
            _authClientConfiguration = clientConfiguration.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
            await context!.Database.EnsureCreatedAsync(cancellationToken);

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (
                await manager!.FindByClientIdAsync(_authClientConfiguration.Id, cancellationToken)
                == null
            )
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ApplicationType = ApplicationTypes.Web,
                    ClientId = _authClientConfiguration.Id,
                    DisplayName = _authClientConfiguration.DisplayName,
                    ClientType = ClientTypes.Public,
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Authorization,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles
                    },
                    Requirements = { Requirements.Features.ProofKeyForCodeExchange }
                };

                _authClientConfiguration
                    .RedirectUris.Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(o => new Uri(o))
                    .ToList()
                    .ForEach(uri => descriptor.RedirectUris.Add(uri));

                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
