using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pipegram.Binders;

namespace Pipegram.Routing;

public static class CallbackQueryRouter_ServiceCollectionExtensions
{
    public static IServiceCollection AddCallbackQueryRouting(this IServiceCollection services)
    {
        services.AddRouting();
        services.TryAddSingleton<IControllerFactoryBinder, DefaultControllerFactoryBinder>();
        services.TryAddSingleton<IControllerActionBinder, DefaultControllerActionBinder>();
        services.TryAddSingleton<IActionControllerEndpointResolver, DefaultActionControllerEndpointResolver>();
        services.TryAddSingleton<ICallbackQueryEndpointDictionary, CallbackQueryEndpointDictionary>();
        services.TryAddSingleton<ICallbackQueryRouter>(provider =>
        {
            var router = provider.GetRequiredService<IRouterDictionary>();
            var callbackQueryRouter = new CallbackQueryRouter(provider.GetRequiredService<ICallbackQueryEndpointDictionary>());
            router.Add(UpdateType.CallbackQuery, callbackQueryRouter);
            return callbackQueryRouter;
        });
        return services;
    }
}
