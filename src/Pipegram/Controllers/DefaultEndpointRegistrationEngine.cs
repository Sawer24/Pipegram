using Pipegram.Routing;

namespace Pipegram.Controllers;

public class DefaultEndpointRegistrationEngine(IEndpointRegistrarProvider endpointRegistrarProvider)
    : IEndpointRegistrationEngine
{
    private readonly IEndpointRegistrarProvider _endpointRegistrarProvider = endpointRegistrarProvider;
    
    public void RegisterAllEndpoints(Type controllerType)
    {
        var controllerMetadata = CollectMetadata(controllerType.GetCustomAttributes(true));
        foreach (var method in controllerType.GetMethods())
        {
            var attributes = method.GetCustomAttributes(true);
            var metadataCollection = new EndpointMetadataCollection(controllerMetadata.Concat(CollectMetadata(attributes)));
            foreach (var attribute in attributes)
            {
                if (attribute is IEndpointDefinitionAttribute endpointDefinitionAttribute)
                {
                    var registrar = _endpointRegistrarProvider.GetRegistrar(attribute.GetType());
                    registrar.RegisterEndpoint(controllerType, method, metadataCollection, endpointDefinitionAttribute);
                }
            }
        }
    }

    private static object[] CollectMetadata(object[] attributes)
    {
        var metadata = default(List<object>);
        foreach (var attribute in attributes)
            if (attribute is IEndpointMetadataAttribute metadataAttribute)
                (metadata ??= []).AddRange(metadataAttribute.GetMetadata());
        if (metadata is null)
            return [];
        return [.. metadata];
    }
}
