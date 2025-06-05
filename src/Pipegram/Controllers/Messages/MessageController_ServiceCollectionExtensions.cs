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
        services.TryAddSingleton<IEndpointRegistrar<TextMessageActionAttribute>, DefaultTextMessageEndpointRegistrar>();
        return services;
    }
}
