namespace Pipegram.Routing;

public class EndpointMiddleware(UpdateDelegate next)
{
    private readonly UpdateDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public Task Invoke(UpdateContext context)
    {
        var endpoint = context.GetEndpoint();
        
        if (endpoint is null)
            return _next(context);

        if (endpoint.UpdateDelegate is null)
            return Task.CompletedTask;

        return endpoint.UpdateDelegate.Invoke(context);
    }
}
