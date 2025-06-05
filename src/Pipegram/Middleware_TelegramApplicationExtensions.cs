using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Pipegram;

/// <summary>
/// Extension methods for adding middleware.
/// </summary>
public static class Middleware_TelegramApplicationExtensions
{
    /// <summary>
    /// Adds a middleware type to the bot's update pipeline.
    /// </summary>
    /// <typeparam name="TMiddleware">The middleware type.</typeparam>
    /// <param name="application">The <see cref="ITelegramApplication"/> instance.</param>
    /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
    /// <returns>The <see cref="ITelegramApplication"/> instance.</returns>
    public static ITelegramApplication UseMiddleware<TMiddleware>(this ITelegramApplication application, params object[] args)
        => application.UseMiddleware(typeof(TMiddleware), args);

    /// <summary>
    /// Adds a middleware type to the bot's update pipeline.
    /// </summary>
    /// <param name="application">The <see cref="ITelegramApplication"/> instance.</param>
    /// <param name="middleware">The middleware type.</param>
    /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
    /// <returns>The <see cref="ITelegramApplication"/> instance.</returns>
    public static ITelegramApplication UseMiddleware(this ITelegramApplication application, Type middleware, params object[] args)
    {
        application.Use(next =>
        {
            var ctorArgs = new object[args.Length + 1];
            ctorArgs[0] = next;
            Array.Copy(args, 0, ctorArgs, 1, args.Length);
            var instance = ActivatorUtilities.CreateInstance(application.Services, middleware, ctorArgs);
            return MiddlewareInvokeBinder.CreateUpdateDelegate(instance);
        });
        return application;
    }
}

internal static class MiddlewareInvokeBinder
{
    private const string InvokeMethodName = "Invoke";

    public static UpdateDelegate CreateUpdateDelegate(object instance)
    {
        var middleware = instance.GetType();

        var method = middleware.GetMethod(InvokeMethodName, BindingFlags.Instance | BindingFlags.Public)
            ?? throw new InvalidOperationException($"Type '{middleware}' does not have a public method named '{InvokeMethodName}'.");
        if (!typeof(Task).IsAssignableFrom(method.ReturnType))
            throw new InvalidOperationException($"Type '{middleware}' must have a single public method named '{InvokeMethodName}' that returns Task.");

        var parameters = method.GetParameters();
        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(UpdateContext))
            return method.CreateDelegate<UpdateDelegate>(instance);

        var contextParameter = Expression.Parameter(typeof(UpdateContext), "context");

        var services = Expression.Property(contextParameter, nameof(UpdateContext.Services));
        var getServiceMethod = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService))
            ?? throw new InvalidOperationException("Failed to find IServiceProvider.GetService method.");

        var expressions = new Expression[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(UpdateContext))
            {
                expressions[i] = contextParameter;
                continue;
            }
            expressions[i] = Expression.TypeAs(
                Expression.Call(services, getServiceMethod, Expression.Constant(parameters[i].ParameterType)),
                parameters[i].ParameterType);
        }

        var call = (Expression)Expression.Call(Expression.Constant(instance), method, expressions);
        var lambda = Expression.Lambda<UpdateDelegate>(call, contextParameter);
        return lambda.Compile();
    }
}
