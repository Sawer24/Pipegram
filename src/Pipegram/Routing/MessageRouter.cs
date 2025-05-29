using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pipegram.Routing;

public class MessageRouter(IMessageEndpointDictionary endpointCollection) : IMessageRouter
{
    private readonly IMessageEndpointDictionary _endpointCollection = endpointCollection;

    public Task Match(UpdateContext context)
    {
        if (context.Update.Type != UpdateType.Message
            || context.Update.Message is not Message message)
            return Task.CompletedTask;

        var args = message.Text is null ? []
            : message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (_endpointCollection.TryGetValue(args, out var endpoint, out args))
        {
            context.SetEndpoint(endpoint);
            context.Items[ActionRouter.ActionArgsKey] = args;
        }
        return Task.CompletedTask;
}
}
