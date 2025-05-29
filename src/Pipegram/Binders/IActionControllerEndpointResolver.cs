using Pipegram.Routing;

namespace Pipegram.Binders;

public interface IActionControllerEndpointResolver
{
    public (string actionName, IEndpoint endpoint)[] ResolveEndpoints(Type controllerType);
}
