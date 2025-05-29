using Microsoft.Extensions.DependencyInjection;
using Pipegram.Binders;
using Pipegram.Controllers;
using Pipegram.Routing;
using System.Linq.Expressions;
using System.Reflection;

namespace Pipegram;

/// <summary>
/// Extension methods for adding middleware.
/// </summary>
public static class Middleware_TelegramBotExtensions
{
    /// <summary>
    /// Adds a middleware delegate defined in-line to the bot's update pipeline.
    /// </summary>
    /// <param name="bot">The <see cref="ITelegramBot"/> instance.</param>
    /// <param name="middleware">A function that handles the update and calls the given next function.</param>
    /// <returns>The <see cref="ITelegramBot"/> instance.</returns>
    public static ITelegramBot Use(this ITelegramBot bot, Func<UpdateContext, Func<Task>, Task> middleware)
        => bot.Use(next => context => middleware(context, () => next(context)));

    /// <summary>
    /// Adds a middleware delegate defined in-line to the bot's update pipeline.
    /// </summary>
    /// <param name="bot">The <see cref="ITelegramBot"/> instance.</param>
    /// <param name="middleware">A function that handles the update and calls the given next function.</param>
    /// <returns>The <see cref="ITelegramBot"/> instance.</returns>
    public static ITelegramBot Use(this ITelegramBot bot, Func<UpdateContext, UpdateDelegate, Task> middleware)
        => bot.Use(next => context => middleware(context, next));

    /// <summary>
    /// Adds a middleware type to the bot's update pipeline.
    /// </summary>
    /// <typeparam name="TMiddleware">The middleware type.</typeparam>
    /// <param name="bot">The <see cref="ITelegramBot"/> instance.</param>
    /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
    /// <returns>The <see cref="ITelegramBot"/> instance.</returns>
    public static ITelegramBot UseMiddleware<TMiddleware>(this ITelegramBot bot, params object[] args)
        => bot.UseMiddleware(typeof(TMiddleware), args);

    /// <summary>
    /// Adds a middleware type to the bot's update pipeline.
    /// </summary>
    /// <param name="bot">The <see cref="ITelegramBot"/> instance.</param>
    /// <param name="middleware">The middleware type.</param>
    /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
    /// <returns>The <see cref="ITelegramBot"/> instance.</returns>
    public static ITelegramBot UseMiddleware(this ITelegramBot bot, Type middleware, params object[] args)
    {
        return bot.Use(next =>
        {
            var ctorArgs = new object[args.Length + 1];
            ctorArgs[0] = next;
            Array.Copy(args, 0, ctorArgs, 1, args.Length);
            var instance = ActivatorUtilities.CreateInstance(bot.Services, middleware, ctorArgs);
            return MiddlewareInvokeBinder.CreateUpdateDelegate(instance);
        });
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
            expressions[i] = Expression.TypeAs(
                Expression.Call(services, getServiceMethod, Expression.Constant(parameters[i].ParameterType)),
                parameters[i].ParameterType);

        var call = (Expression)Expression.Call(Expression.Constant(instance), method, expressions);
        var lambda = Expression.Lambda<UpdateDelegate>(call, contextParameter);
        return lambda.Compile();
    }
}
