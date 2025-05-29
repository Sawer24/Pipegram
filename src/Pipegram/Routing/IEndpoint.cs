namespace Pipegram.Routing;

public interface IEndpoint
{
    public string? DisplayName { get; }

    public EndpointMetadataCollection Metadata { get; }

    public UpdateDelegate? UpdateDelegate { get; }
}
