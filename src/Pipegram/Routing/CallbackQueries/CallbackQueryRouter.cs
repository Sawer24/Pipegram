using Telegram.Bot.Types;

namespace Pipegram.Routing.CallbackQueries;

public class CallbackQueryRouter(ICallbackQueryEndpointDictionary endpointCollection) : IRouter
{
    private readonly ICallbackQueryEndpointDictionary _endpointCollection = endpointCollection;

    public Task Match(UpdateContext context)
    {
        if (context.Update.CallbackQuery is not CallbackQuery callbackQuery
            || callbackQuery.Data is not string queryData)
            return Task.CompletedTask;

        var args = queryData.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var endpointName = args[0];
        if (_endpointCollection.TryGetValue(endpointName, out var endpoint))
        {
            context.SetEndpoint(endpoint);
            context.Items[RouterConstants.ActionArgsKey] = args[1..];
        }
        return Task.CompletedTask;
    }
}
