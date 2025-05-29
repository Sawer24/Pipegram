using Microsoft.Extensions.DependencyInjection;
using Pipegram.Binders;

namespace Pipegram.Routing;

public static class Message_TelegramBotExtensions
{
    public static ITelegramBot MapMessage(this ITelegramBot bot, string message, Delegate handler)
    {
        _ = bot.Services.GetRequiredService<IMessageRouter>();
        var dictionary = bot.Services.GetRequiredService<IMessageEndpointDictionary>();
        var binder = bot.Services.GetService<IActionUpdateDelegateBinder>()
            ?? new DefaultActionUpdateDelegateBinder();
        var updateDelegate = binder.CreateActionUpdateDelegate(handler);
        var endpoint = new Endpoint(updateDelegate, null, message);
        dictionary.Add(message.Split(' '), endpoint);
        return bot;
    }
}
