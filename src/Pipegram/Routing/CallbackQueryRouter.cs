using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Pipegram.Routing;

public class CallbackQueryRouter(ICallbackQueryEndpointDictionary endpointCollection) : ICallbackQueryRouter
{
    private readonly ICallbackQueryEndpointDictionary _endpointCollection = endpointCollection;

    public Task Match(UpdateContext context)
    {
        if (context.Update.Type != UpdateType.CallbackQuery
            || context.Update.CallbackQuery is not CallbackQuery callbackQuery
            || callbackQuery.Data is not string queryData)
            return Task.CompletedTask;

        var args = queryData.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var endpointName = args[0];
        if (_endpointCollection.TryGetValue(endpointName, out var endpoint))
        {
            context.SetEndpoint(endpoint);
            context.Items[ActionRouter.ActionArgsKey] = args[1..];
        }
        return Task.CompletedTask;
    }
}
