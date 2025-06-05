using Microsoft.Extensions.DependencyInjection;
using Pipegram.Binders;
using Pipegram.Routing.Messages;

namespace Pipegram.Routing;

public static class Message_TelegramBotExtensions
{
    public static ITelegramApplication MapMessage(this ITelegramApplication application, string message, Delegate handler)
    {
        var dictionary = application.Services.GetRequiredService<ITextMessageEndpointDictionary>();
        var binder = application.Services.GetService<IActionUpdateDelegateBinder>()
            ?? new DefaultActionUpdateDelegateBinder();
        var updateDelegate = binder.CreateActionUpdateDelegate(handler);
        var endpoint = new Endpoint(updateDelegate, null, message);
        dictionary.Add(message.Split(' '), endpoint);
        return application;
    }
}
