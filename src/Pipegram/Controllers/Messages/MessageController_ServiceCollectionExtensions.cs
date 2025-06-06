using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pipegram.Routing.Messages;

namespace Pipegram.Controllers.Messages;

public static class MessageController_ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMessageRouting();
        services.TryAddSingleton<DefaultTextMessageEndpointRegistrar>();
        services.TryAddSingleton<IEndpointRegistrar<TextMessageActionAttribute>>(p => p.GetRequiredService<DefaultTextMessageEndpointRegistrar>());
        services.TryAddSingleton<IEndpointRegistrar<StartActionAttribute>>(p => p.GetRequiredService<DefaultTextMessageEndpointRegistrar>());
        return services;
    }
}
