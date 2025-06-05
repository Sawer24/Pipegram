namespace Pipegram.Routing;

public class EndpointRoutingMiddleware(UpdateDelegate next, IEnumerable<IRouter> routers)
{
    private readonly UpdateDelegate _next = next;
    private readonly IRouter[] _routers = [.. routers];

    public Task Invoke(UpdateContext context)
    {
        for (var i = 0; context.GetEndpoint() is null && i < _routers.Length; i++)
        {
            var matchTask = _routers[i].Match(context);
            if (!matchTask.IsCompletedSuccessfully)
                return InvokeAsync(context, matchTask, i + 1);
        }
        return _next(context);
    }

    private async Task InvokeAsync(UpdateContext context, Task lastMatch, int i)
    {
        await lastMatch;
        for (; context.GetEndpoint() is null && i < _routers.Length; i++)
            await _routers[i].Match(context);
        await _next(context);
    }
}
