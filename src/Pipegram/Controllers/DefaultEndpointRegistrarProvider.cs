namespace Pipegram.Controllers;

public class DefaultEndpointRegistrarProvider(IServiceProvider serviceProvider) : IEndpointRegistrarProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IEndpointRegistrar GetRegistrar(Type attributeType)
    {
        ArgumentNullException.ThrowIfNull(attributeType);

        var registrarType = typeof(IEndpointRegistrar<>).MakeGenericType(attributeType);
        var registrar = _serviceProvider.GetService(registrarType)
            ?? throw new InvalidOperationException($"No endpoint registrar found for attribute type '{attributeType.FullName}'.");
        return (IEndpointRegistrar)registrar;
    }
}
