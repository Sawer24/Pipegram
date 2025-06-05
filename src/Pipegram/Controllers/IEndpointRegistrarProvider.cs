namespace Pipegram.Controllers;

public interface IEndpointRegistrarProvider
{
    IEndpointRegistrar GetRegistrar(Type attributeType);
}
