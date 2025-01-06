using Microsoft.Extensions.DependencyInjection;

namespace Autoflow.Portal.Base._Internals
{
    internal static class ServiceDescriptorExtensions
    {
        public static object? NormalizedImplementationInstance(this ServiceDescriptor descriptor) =>
            descriptor.IsKeyedService
                ? descriptor.KeyedImplementationInstance
                : descriptor.ImplementationInstance;
    }
}
