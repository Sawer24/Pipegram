using Pipegram.Controllers;
using Pipegram.Routing;
using System.Reflection;

namespace Pipegram.Binders;

public class DefaultActionControllerEndpointResolver(IControllerFactoryBinder controllerFactoryArgumentBinder, IControllerActionBinder controllerActionBinder)
    : IMessageControllerEndpointResolver, ICallbackQueryControllerEndpointResolver
{
    private readonly IControllerFactoryBinder _controllerFactoryBinder = controllerFactoryArgumentBinder;
    private readonly IControllerActionBinder _controllerActionBinder = controllerActionBinder;

    public (string actionName, IEndpoint endpoint)[] ResolveEndpoints(Type controllerType)
    {
        if (!typeof(TelegramControllerBase).IsAssignableFrom(controllerType))
            throw new ArgumentException($"Type '{controllerType}' must inherit {nameof(TelegramControllerBase)}.", nameof(controllerType));

        var controllerFactory = _controllerFactoryBinder.CreateFactory(controllerType);

        var routeAttribute = controllerType.GetCustomAttribute<RouteAttribute>();

        var actions = new List<(ActionAttribute actionAttribute, MethodInfo method)>();
        foreach (var method in controllerType.GetMethods())
        {
            if (method.IsSpecialName)
                continue;
            foreach (var actionAttribute in method.GetCustomAttributes<ActionAttribute>())
                actions.Add((actionAttribute, method));
        }

        var controllerMetadata = GetMetadata(controllerType);
        var endpoints = new (string actionName, IEndpoint endpoint)[actions.Count];
        for (var i = 0; i < actions.Count; i++)
        {
            var (actionAttribute, method) = actions[i];
            var name = routeAttribute?.Route + actionAttribute.Name;
            var metadata = (object[])[.. controllerMetadata, .. GetMetadata(method)];
            var invoker = _controllerActionBinder.CreateActionDelegate(controllerType, method);
            UpdateDelegate updateDelegate = method.IsStatic ? StaticUpdateDelegate : UpdateDelegate;
            var endpoint = new Endpoint(updateDelegate, new EndpointMetadataCollection(metadata), name);
            endpoints[i] = (name, endpoint);

            Task StaticUpdateDelegate(UpdateContext context) => invoker(null, context);
            Task UpdateDelegate(UpdateContext context) => invoker(controllerFactory(context).Initialize(context), context);
        }
        return endpoints;
    }

    private static object[] GetMetadata(MemberInfo member)
    {
        var attributes = member.GetCustomAttributes<EndpointMetadataAttribute>(true);
        var metadata = default(List<object>);
        foreach (var attribute in attributes)
            (metadata ??= []).AddRange(attribute.GetMetadata());
        if (metadata is null)
            return [];
        return [.. metadata];
    }
}
