using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pipegram.Binders;
using Pipegram.Interceptions;

namespace Pipegram.Routing;

public static class MessageRouter_ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageRouting(this IServiceCollection services)
    {
        services.AddRouting();
        services.TryAddSingleton<IMessageInterceptor, MessageInterceptor>();
        services.TryAddSingleton<ICallbackQueryInterceptor, CallbackQueryInterceptor>();
        services.TryAddSingleton<IControllerFactoryBinder, DefaultControllerFactoryBinder>();
        services.TryAddSingleton<IControllerActionBinder, DefaultControllerActionBinder>();
        services.TryAddSingleton<IActionControllerEndpointResolver, DefaultActionControllerEndpointResolver>();
        services.TryAddSingleton<IMessageEndpointDictionary, MessageEndpointDictionary>();
        services.TryAddSingleton<IMessageRouter>(provider =>
        {
            var router = provider.GetRequiredService<IRouterDictionary>();
            var messageRouter = new MessageRouter(provider.GetRequiredService<IMessageEndpointDictionary>());
            router.Add(UpdateType.Message, messageRouter);
            return messageRouter;
        });
        return services;
    }
}
