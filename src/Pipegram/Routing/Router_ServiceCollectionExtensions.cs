using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pipegram.Routing;

public static class Router_ServiceCollectionExtensions
{
    public static IServiceCollection AddRouting(this IServiceCollection services)
    {
        services.TryAddSingleton<IRouterDictionary, RouterDictionary>();
        return services;
    }
}
