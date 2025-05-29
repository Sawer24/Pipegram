namespace Pipegram.Controllers;

public abstract class EndpointMetadataAttribute : Attribute
{
    public virtual object[] GetMetadata() => [this];
}
