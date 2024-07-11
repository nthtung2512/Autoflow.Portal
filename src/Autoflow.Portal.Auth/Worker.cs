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
            if (await manager.FindByClientIdAsync(_authenticationEndPoint.ClientId) == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = _authenticationEndPoint.ClientId,
                    ClientSecret = _authenticationEndPoint.ClientSecret,
                    DisplayName = _authenticationEndPoint.DisplayName,
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
