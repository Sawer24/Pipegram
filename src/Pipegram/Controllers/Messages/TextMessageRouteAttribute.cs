namespace Pipegram.Controllers.Messages;

[AttributeUsage(AttributeTargets.Class)]
public class TextMessageRouteAttribute(string route) : EndpointMetadataBaseAttribute, ITextMessageRouteMetadata
{
    public string Route { get; } = route;
}
