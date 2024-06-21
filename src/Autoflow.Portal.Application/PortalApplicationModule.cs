using Autoflow.Portal.Application.Contracts.Repositories;
using Autoflow.Portal.Base;
using Autoflow.Portal.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autoflow.Portal.Application
{
    public class PortalApplicationModule : Module
    {
        public override void ConfigureService(IHostApplicationBuilder builder)
        {
            //builder.Services.AddScoped<IRunnerBotRepository, RunnerBotRepository>();
            builder.Services.AddScoped<IChatBoxRepository, ChatBoxRepository>();
        }
    }
}
