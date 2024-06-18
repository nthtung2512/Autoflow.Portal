using Microsoft.Extensions.Hosting;

namespace Autoflow.Portal.Base
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddModules<TModule>(this IHostApplicationBuilder builder)
            where TModule : Module, new()
        {
            var module = new TModule();
            module.ConfigureService(builder);
        }
    }
}
