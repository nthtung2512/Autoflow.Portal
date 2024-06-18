using Autoflow.Portal.Application.Repositories;
using Autoflow.Portal.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autoflow.Portal.Application
{
    public class PortalApplicationModule : Module
    {
        public override void ConfigureService(IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRunnerBotRepository, RunnerBotRepository>();
        }
    }
}
