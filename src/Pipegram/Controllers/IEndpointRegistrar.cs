using Pipegram.Routing;
using System.Reflection;

namespace Pipegram.Controllers;

public interface IEndpointRegistrar
{
    public void RegisterEndpoint(Type controllerType, MethodInfo methodInfo,
        EndpointMetadataCollection metadataCollection, IEndpointDefinitionAttribute definitionAttribute);
}

public interface IEndpointRegistrar<TAttribute> : IEndpointRegistrar
    where TAttribute : IEndpointDefinitionAttribute
{
}
