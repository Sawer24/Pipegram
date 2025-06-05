namespace Pipegram.Controllers;

public interface IEndpointRegistrationEngine
{
    void RegisterAllEndpoints(Type controllerType);
}
