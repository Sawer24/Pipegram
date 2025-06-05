namespace Pipegram.Controllers.Messages;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TextMessageActionAttribute(string name)
    : EndpointMetadataBaseAttribute, IEndpointDefinitionAttribute, ITextMessageActionMetadata
{
    public string Name { get; } = name;
}
