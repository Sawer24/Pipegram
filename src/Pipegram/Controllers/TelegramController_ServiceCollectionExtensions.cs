using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pipegram.Interceptions;

namespace Pipegram.Controllers;

public static class TelegramController_ServiceCollectionExtensions
{
    public static IServiceCollection AddControllers(this IServiceCollection services)
    {
        services.AddInterceptors();
        services.TryAddSingleton<IEndpointRegistrarProvider, DefaultEndpointRegistrarProvider>();
        services.TryAddSingleton<IEndpointRegistrationEngine, DefaultEndpointRegistrationEngine>();
        return services;
    }
}
