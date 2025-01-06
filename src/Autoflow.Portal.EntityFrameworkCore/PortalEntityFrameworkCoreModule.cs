using Autoflow.Portal.Base;
using Autoflow.Portal.Base.Extensions;
using Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autoflow.Portal.EntityFrameworkCore
{
    public class PortalEntityFrameworkCoreModule : Module
    {
        public override void ConfigureService(IServiceCollection services)
        {
            services.AddDbContext<PortalDbContext>(options =>
            {
                options.UseNpgsql(services.GetConfiguration().GetConnectionString("Default"));
                options.UseOpenIddict();
            });
        }
    }
}
