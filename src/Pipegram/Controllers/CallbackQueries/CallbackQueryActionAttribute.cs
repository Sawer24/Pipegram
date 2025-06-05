namespace Pipegram.Controllers.CallbackQueries;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CallbackQueryActionAttribute(string name)
    : EndpointMetadataBaseAttribute, IEndpointDefinitionAttribute, ICallbackQueryActionMetadata
{
    public string Name { get; } = name;
}
