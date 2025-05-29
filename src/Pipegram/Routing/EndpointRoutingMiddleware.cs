namespace Pipegram.Routing;

public class EndpointRoutingMiddleware(UpdateDelegate next, IRouterDictionary routerDictionary)
{
    private readonly UpdateDelegate _next = next;
    private readonly IRouterDictionary _routerDictionary = routerDictionary;

    public async Task Invoke(UpdateContext context)
    {
        if (context.GetEndpoint() is null 
            && _routerDictionary.TryGetValue(context.Update.Type, out var router))
            await router.Match(context);
        await _next(context);
    }
}
