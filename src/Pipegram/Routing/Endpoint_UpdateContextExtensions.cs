namespace Pipegram.Routing;

public static class Endpoint_UpdateContextExtensions
{
    public const string EndpointKey = "__Endpoint";

    public static void SetEndpoint(this UpdateContext context, IEndpoint endpoint) => context.Items[EndpointKey] = endpoint;

    public static IEndpoint? GetEndpoint(this UpdateContext context)
    {
        if (context.Items.TryGetValue(EndpointKey, out var endpoint))
            return endpoint as IEndpoint;
        return null;
    }
}
