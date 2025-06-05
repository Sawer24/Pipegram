namespace Pipegram.Controllers.CallbackQueries;

[AttributeUsage(AttributeTargets.Class)]
public class CallbackQueryRouteAttribute(string route)
    : EndpointMetadataBaseAttribute, ICallbackQueryRouteMetadata
{
    public string Route { get; } = route;
}
