using Microsoft.Extensions.DependencyInjection;
using Pipegram.Binders;
using Pipegram.Routing;

namespace Pipegram.Controllers;

public static class MessageController_TelegramBotExtensions
{
    public static ITelegramBot MapMessageController<TController>(this ITelegramBot bot)
        where TController : MessageControllerBase
        => bot.MapMessageController(typeof(TController));

    public static ITelegramBot MapMessageController(this ITelegramBot bot, Type controllerType)
    {
        if (!typeof(MessageControllerBase).IsAssignableFrom(controllerType))
            throw new ArgumentException($"Type '{controllerType}' is not a valid message controller type.");
        _ = bot.Services.GetRequiredService<IMessageRouter>();
        var dictionary = bot.Services.GetRequiredService<IMessageEndpointDictionary>();
        var resolver = bot.Services.GetRequiredService<IActionControllerEndpointResolver>();
        var endpoints = resolver.ResolveEndpoints(controllerType);
        foreach (var (name, endpoint) in endpoints)
            dictionary.Add(name.Split(' '), endpoint);
        return bot;
    }
}
