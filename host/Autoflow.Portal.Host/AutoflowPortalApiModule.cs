using Autoflow.Portal.Application;
using Autoflow.Portal.Base;
using Autoflow.Portal.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Autoflow.Portal.Host
{
    [DependsOn(
        [
            typeof(PortalApplicationModule),
            typeof(PortalEntityFrameworkCoreModule),
            typeof(PortalAuthModule),
            typeof(PortalDomainModule)
        ]
    )]
    public class AutoflowPortalApiModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {

            // Add services to the container.
            services.AddSignalR();

            // Register services with the dependency injection (DI) container
            services.AddControllers();          // Add services for controllers to the container
            services.AddEndpointsApiExplorer(); // Add services for API endpoint exploration (useful for tools like Swagger)
            services.AddSwaggerGen();           // Add services required to generate Swagger documentation

            services.AddCors((options) =>
            {
                options.AddPolicy("PortalChatBox",
                    new CorsPolicyBuilder()
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .Build());
            });

        }
    }
}
