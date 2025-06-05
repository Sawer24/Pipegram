using Telegram.Bot.Types;

namespace Pipegram.Routing.Messages;

public class MessageRouter(ITextMessageEndpointDictionary textEndpointCollection) : IRouter
{
    private readonly ITextMessageEndpointDictionary _endpointCollection = textEndpointCollection;

    public Task Match(UpdateContext context)
    {
        if (context.Update.Message is not Message message
            || message.Text is not string messageText)
            return Task.CompletedTask;

        var args = messageText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (_endpointCollection.TryGetValue(args, out var endpoint, out args))
        {
            context.SetEndpoint(endpoint);
            context.Items[RouterConstants.ActionArgsKey] = args;
        }
        return Task.CompletedTask;
    }
}
