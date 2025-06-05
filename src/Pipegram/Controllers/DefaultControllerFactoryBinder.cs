using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Pipegram.Controllers;

public class DefaultControllerFactoryBinder : IControllerFactoryBinder
{
    public Func<UpdateContext, object> CreateFactory(Type controllerType)
    {
        var constructors = controllerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        if (constructors.Length == 0)
            throw new ArgumentException($"Type '{controllerType}' must have public constructor.");
        var (_, constructor, parameters) = constructors.Max(c =>
        {
            var parameters = c.GetParameters();
            return (parameters.Length, c, parameters);
        });

        var getRequiredServiceMethod = typeof(ServiceProviderServiceExtensions)
            .GetMethod(nameof(ServiceProviderServiceExtensions.GetRequiredService), 0, [typeof(IServiceProvider), typeof(Type)])
            ?? throw new InvalidOperationException("Failed to find ServiceProviderServiceExtensions.GetRequiredService method.");
        var getServiceMethod = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService))
            ?? throw new InvalidOperationException("Failed to find IServiceProvider.GetService method.");
        var initializeMethod = typeof(ITelegramInitializableController).GetMethod(nameof(ITelegramInitializableController.Initialize))
            ?? throw new InvalidOperationException("Failed to find ITelegramInitializableController.Initialize method.");

        var contextParameter = Expression.Parameter(typeof(UpdateContext), "context");
        var services = Expression.Property(contextParameter, nameof(UpdateContext.Services));

        var arguments = new Expression[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            var service = parameter.HasDefaultValue
                ? Expression.Coalesce(Expression.Call(services, getServiceMethod, Expression.Constant(parameter.ParameterType)),
                    Expression.Constant(parameter.DefaultValue))
                : (Expression)Expression.Call(getRequiredServiceMethod, services, Expression.Constant(parameter.ParameterType));
            arguments[i] = Expression.TypeAs(service, parameter.ParameterType);
        }

        var body = (Expression)Expression.New(constructor, arguments);

        if (typeof(ITelegramInitializableController).IsAssignableFrom(controllerType))
            body = Expression.Call(body, initializeMethod, contextParameter);

        body = Expression.TypeAs(body, controllerType);

        var lambda = Expression.Lambda<Func<UpdateContext, object>>(body, contextParameter);
        return lambda.Compile();
    }
}
