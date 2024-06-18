using Microsoft.Extensions.Hosting;

namespace Autoflow.Portal.Base
{
    public abstract class Module
    {
        public abstract void ConfigureService(IHostApplicationBuilder builder);
    }
}
