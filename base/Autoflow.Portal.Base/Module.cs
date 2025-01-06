using Microsoft.Extensions.DependencyInjection;

namespace Autoflow.Portal.Base
{
    public abstract class Module
    {
        public abstract void ConfigureService(IServiceCollection services);
    }
}
