namespace Pipegram.Controllers;

public abstract class RouteAttribute(string route) : EndpointMetadataAttribute, IRouteMetadata
{
    public string Route { get; } = route;
}
