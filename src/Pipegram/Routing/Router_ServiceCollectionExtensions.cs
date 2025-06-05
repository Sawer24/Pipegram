using Microsoft.Extensions.DependencyInjection;

namespace Pipegram.Routing;

public static class Router_ServiceCollectionExtensions
{
    public static IServiceCollection TryAddRouter<T>(this IServiceCollection services)
        where T : class, IRouter
    {
        if (!services.Any(d => d.ServiceType == typeof(IRouter) && d.ImplementationType == typeof(T)))
            services.AddSingleton<IRouter, T>();
        return services;
    }
}
