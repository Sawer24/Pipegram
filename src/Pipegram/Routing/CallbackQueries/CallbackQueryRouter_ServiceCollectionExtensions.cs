using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pipegram.Routing.CallbackQueries;

public static class CallbackQueryRouter_ServiceCollectionExtensions
{
    public static IServiceCollection AddCallbackQueryRouting(this IServiceCollection services)
    {
        services.TryAddSingleton<ICallbackQueryEndpointDictionary, CallbackQueryEndpointDictionary>();
        services.TryAddRouter<CallbackQueryRouter>();
        return services;
    }
}
