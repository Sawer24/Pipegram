namespace Pipegram.Routing;

public class EndpointMiddleware(UpdateDelegate next)
{
    private readonly UpdateDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public Task Invoke(UpdateContext context)
    {
        var endpoint = context.GetEndpoint();
        return endpoint?.UpdateDelegate?.Invoke(context)
            ?? _next(context);
    }
}
