using Microsoft.Extensions.DependencyInjection;
using Pipegram.Binders;
using Pipegram.Routing;

namespace Pipegram.Controllers;

public static class CallbackQueryController_TelegramBotExtensions
{
    public static ITelegramBot MapCallbackQueryController<TController>(this ITelegramBot bot)
        where TController : CallbackQueryControllerBase
        => bot.MapCallbackController(typeof(TController));

    public static ITelegramBot MapCallbackController(this ITelegramBot bot, Type controllerType)
    {
        if (!typeof(CallbackQueryControllerBase).IsAssignableFrom(controllerType))
            throw new ArgumentException($"Type '{controllerType}' is not a valid callback query controller type.");
        _ = bot.Services.GetRequiredService<ICallbackQueryRouter>();
        var dictionary = bot.Services.GetRequiredService<ICallbackQueryEndpointDictionary>();
        var resolver = bot.Services.GetRequiredService<IActionControllerEndpointResolver>();
        var endpoints = resolver.ResolveEndpoints(controllerType);
        foreach (var (name, endpoint) in endpoints)
        {
            if (name.Contains(' '))
                throw new ArgumentException($"Endpoint name '{name}' contains spaces, which is not allowed for callback query endpoints.");
            dictionary.Add(name, endpoint);
        }
        return bot;
    }
}
