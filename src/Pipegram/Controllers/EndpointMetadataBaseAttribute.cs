namespace Pipegram.Controllers;

public abstract class EndpointMetadataBaseAttribute : Attribute, IEndpointMetadataAttribute
{
    public virtual object[] GetMetadata() => [this];
}
