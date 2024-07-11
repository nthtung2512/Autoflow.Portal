using Autoflow.Portal.Base;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autoflow.Portal.EntityFrameworkCore
{
    public class PortalEntityFrameworkCoreModule : Module
    {
        public override void ConfigureService(IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<PortalDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
                options.UseOpenIddict();
            });
        }
    }
}
