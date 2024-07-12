using Autoflow.Portal.Auth.Configurations;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Autoflow.Portal.Auth
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthenticationEndPoint _authenticationEndPoint;

        public Worker(IServiceProvider serviceProvider, IOptions<AuthenticationEndPoint> clientConfiguration)
        {
            _serviceProvider = serviceProvider;
            _authenticationEndPoint = clientConfiguration.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // The paramter "chathub" is the client_id of the client application.
            // This one creates new client application if it doesn't exist.
            if (await manager.FindByClientIdAsync("chathub") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "chathub",
                    ClientSecret = "293f9bac-8cb1-4b59-8756-1b5193526980",
                    DisplayName = "ChatHub",
                    Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.Scopes.Roles
                }
                });
            }
            if (await manager.FindByClientIdAsync("chathub1") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "chathub1",
                    ClientSecret = "d0270316-b546-4c2d-9726-ff2c1eb7f25d",
                    DisplayName = "ChatHub1",
                    Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.Scopes.Roles
                }
                });
            }
            if (await manager.FindByClientIdAsync("chathub2") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "chathub2",
                    ClientSecret = "594f2eef-5d96-47ab-9e35-062934c68d4b",
                    DisplayName = "ChatHub2",
                    Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.Scopes.Roles
                }
                });
            }
            if (await manager.FindByClientIdAsync("chathubAdmin") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "chathubAdmin",
                    ClientSecret = "60602bff-f912-4a6d-8ba1-c2f5b543cd71",
                    DisplayName = "ChatAdmin",
                    Permissions =
                {
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.ClientCredentials,
                    Permissions.Scopes.Roles
                }
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
