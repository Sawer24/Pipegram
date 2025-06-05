using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pipegram.Routing.Messages;

public static class MessageRouter_ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageRouting(this IServiceCollection services)
    {
        services.TryAddSingleton<ITextMessageEndpointDictionary, TextMessageEndpointDictionary>();
        services.TryAddRouter<MessageRouter>();
        return services;
    }
}
