namespace Pipegram.Controllers;

public abstract class ActionAttribute(string name) : EndpointMetadataAttribute, IActionMetadata
{
    public string Name { get; } = name;
}
