using Autoflow.Portal.Application;
using Autoflow.Portal.Base;
using Autoflow.Portal.EntityFrameworkCore;

namespace Autoflow.Portal.Host
{
    public class AutoflowPortalApiModule : Module
    {
        private readonly Module[] DependentType = [
            new PortalApplicationModule(),
            new PortalEntityFrameworkCoreModule()
        ];

        public override void ConfigureService(IHostApplicationBuilder builder)
        {
            foreach (var type in DependentType)
            {
                type.ConfigureService(builder);
            }
        }
    }
}
