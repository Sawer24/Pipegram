using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pipegram.Routing.CallbackQueries;

namespace Pipegram.Controllers.CallbackQueries;

public static class CallbackQueryController_ServiceCollectionExtensions
{
    public static IServiceCollection AddCallbackQueryControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddCallbackQueryRouting();
        services.TryAddSingleton<IEndpointRegistrar<CallbackQueryActionAttribute>, DefaultCallbackQueryEndpointRegistrar>();
        return services;
    }
}
