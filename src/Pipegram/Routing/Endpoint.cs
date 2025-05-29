namespace Pipegram.Routing;

public class Endpoint(UpdateDelegate? updateDelegate, EndpointMetadataCollection? metadata = null, string? displayName = null) : IEndpoint
{
    public string? DisplayName { get; } = displayName;
    public EndpointMetadataCollection Metadata { get; } = metadata ?? EndpointMetadataCollection.Empty;
    public UpdateDelegate? UpdateDelegate { get; } = updateDelegate;
}
