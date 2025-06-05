using Pipegram.Controllers.CallbackQueries;
using Pipegram.Routing;
using Pipegram.Routing.Messages;
using System.Linq.Expressions;
using System.Reflection;

namespace Pipegram.Controllers.Messages;

public class DefaultTextMessageEndpointRegistrar(ITextMessageEndpointDictionary textMessageEndpointDictionary,
    IControllerFactoryBinder? controllerFactoryBinder = null) : IEndpointRegistrar<TextMessageActionAttribute>
{
    private readonly ITextMessageEndpointDictionary _textMessageEndpointDictionary = textMessageEndpointDictionary;
    private readonly IControllerFactoryBinder _controllerFactoryBinder = controllerFactoryBinder
        ?? new DefaultControllerFactoryBinder();

    public void RegisterEndpoint(Type controllerType, MethodInfo methodInfo,
        EndpointMetadataCollection metadataCollection, IEndpointDefinitionAttribute definitionAttribute)
    {
        if (definitionAttribute is not TextMessageActionAttribute actionAttribute)
            throw new ArgumentException($"Expected {nameof(TextMessageActionAttribute)}, but got {definitionAttribute.GetType()}.", nameof(definitionAttribute));

        var controllerFactory = methodInfo.IsStatic ? null
            : _controllerFactoryBinder.CreateFactory(controllerType);

        var routeAttribute = metadataCollection.GetMetadata<ICallbackQueryRouteMetadata>();
        var name = routeAttribute?.Route + actionAttribute.Name;
        var keys = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var invoker = CreateActionDelegate(controllerType, methodInfo);
        UpdateDelegate updateDelegate = methodInfo.IsStatic ? StaticUpdateDelegate : UpdateDelegate;
        var endpoint = new Endpoint(updateDelegate, metadataCollection, name);
        _textMessageEndpointDictionary.Add(keys, endpoint);

        Task StaticUpdateDelegate(UpdateContext context) => invoker(null, context);
        Task UpdateDelegate(UpdateContext context) => invoker(controllerFactory!(context), context);
    }

    private static Func<object?, UpdateContext, Task> CreateActionDelegate(Type controllerType, MethodInfo method)
    {
        var controllerParameter = Expression.Parameter(typeof(TelegramControllerBase), "controller");
        var contextParameter = Expression.Parameter(typeof(UpdateContext), "context");

        var parameters = method.GetParameters();

        var services = Expression.Property(contextParameter, nameof(UpdateContext.Services));
        var getServiceMethod = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService))
            ?? throw new InvalidOperationException("Failed to find IServiceProvider.GetService method.");

        var argsVariable = Expression.Variable(typeof(string[]), "args");
        var setArgs = Expression.Assign(argsVariable,
            Expression.TypeAs(Expression.Property(Expression.Property(contextParameter, nameof(UpdateContext.Items)), "Item",
            Expression.Constant(RouterConstants.ActionArgsKey)), typeof(string[])));

        var argsId = 0;
        var expressions = new Expression[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
            if (parameters[i].ParameterType == typeof(string))
            {
                expressions[i] = Expression.Condition(Expression.LessThan(Expression.Constant(argsId), Expression.ArrayLength(argsVariable)),
                    Expression.ArrayAccess(argsVariable, Expression.Constant(argsId)),
                    Expression.Constant(null, typeof(string)));
                argsId++;
            }
            else
                expressions[i] = Expression.TypeAs(
                    Expression.Call(services, getServiceMethod, Expression.Constant(parameters[i].ParameterType)),
                    parameters[i].ParameterType);
        Expression call;
        if (!method.IsStatic)
            call = Expression.Call(Expression.TypeAs(controllerParameter, controllerType), method, expressions);
        else
            call = Expression.Call(method, expressions);

        if (typeof(IResult).IsAssignableFrom(call.Type))
            call = Expression.Call(call, typeof(IResult).GetMethod(nameof(IResult.Execute))!, contextParameter);

        if (call.Type.IsGenericType && call.Type.GetGenericTypeDefinition() == typeof(Task<>)
            && typeof(IResult).IsAssignableFrom(call.Type.GetGenericArguments()[0]))
        {
            var executeAsync = typeof(DefaultTextMessageEndpointRegistrar).GetMethod(nameof(ExecuteAsync),
               BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)!;
            call = Expression.Call(executeAsync, call, contextParameter);
        }

        if (!typeof(Task).IsAssignableFrom(call.Type))
            call = Expression.Block(call, Expression.Constant(Task.CompletedTask));

        var body = Expression.Block([argsVariable], setArgs, call);
        var lambda = Expression.Lambda<Func<object?, UpdateContext, Task>>(body, controllerParameter, contextParameter);
        return lambda.Compile();
    }

    private static async Task ExecuteAsync(Task<IResult> task, UpdateContext context) => await (await task).Execute(context);
}
