namespace Pipegram.Interceptions;

public class MessageInterceptorMiddleware(UpdateDelegate next, IMessageInterceptor messageInterceptor)
{
    private readonly UpdateDelegate _next = next;
    private readonly IMessageInterceptor _messageInterceptor = messageInterceptor;

    public Task Invoke(UpdateContext context)
    {
        if (_messageInterceptor.TryUseInterceptor(context))
            return Task.CompletedTask;

        return _next(context);
    }
}
