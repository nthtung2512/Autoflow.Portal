using Microsoft.Extensions.DependencyInjection;

namespace Autoflow.Portal.Base
{
    public static class ApplicationBuilderExtensions
    {
        public static void AddModules<TModule>(this IServiceCollection services)
            where TModule : Module, new()
        {
            ResolveDependentModules(typeof(TModule), services);
        }

        private static void ResolveDependentModules(Type currentType, IServiceCollection services)
        {
            var dependsOns = currentType
                .GetCustomAttributes(typeof(DependsOnAttribute), false)
                .FirstOrDefault();

            if (dependsOns is DependsOnAttribute dependants)
            {
                foreach (var dependType in dependants.DependedTypes)
                {
                    if (dependType.IsSubclassOf(typeof(Module)))
                    {
                        ResolveDependentModules(dependType, services);
                    }
                }
            }

            var currentInstance = Activator.CreateInstance(currentType) as Module;
            currentInstance?.ConfigureService(services);
        }
    }
}
