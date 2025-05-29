using Pipegram.Controllers;
using System.Linq.Expressions;
using System.Reflection;

namespace Pipegram.Binders;

public class DefaultControllerFactoryBinder : IControllerFactoryBinder
{
    public Func<UpdateContext, TelegramControllerBase> CreateFactory(Type controllerType)
    {
        var constructors = controllerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        if (constructors.Length != 1)
            throw new ArgumentException($"Type '{controllerType}' must have a single public constructor.");
        var constructor = constructors[0];

        var contextParameter = Expression.Parameter(typeof(UpdateContext), "context");

        var parameters = constructor.GetParameters();
        var services = Expression.Property(contextParameter, nameof(UpdateContext.Services));
        var getServiceMethod = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService))
            ?? throw new InvalidOperationException("Failed to find IServiceProvider.GetService method.");

        var expressions = new Expression[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
            expressions[i] = Expression.TypeAs(
                Expression.Call(services, getServiceMethod, Expression.Constant(parameters[i].ParameterType)),
                parameters[i].ParameterType);

        var call = Expression.TypeAs(Expression.New(constructor, expressions), controllerType);
        var lambda = Expression.Lambda<Func<UpdateContext, TelegramControllerBase>>(call, contextParameter);
        return lambda.Compile();
    }
}
