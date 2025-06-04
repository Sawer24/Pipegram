namespace Pipegram.Interceptions;

public class CallbackQueryInterceptorMiddleware(UpdateDelegate next, ICallbackQueryInterceptor callbackQueryInterceptor)
{
    private readonly UpdateDelegate _next = next;
    private readonly ICallbackQueryInterceptor _callbackQueryInterceptor = callbackQueryInterceptor;

    public Task Invoke(UpdateContext context)
    {
        if (_callbackQueryInterceptor.TryUseInterceptor(context))
            return Task.CompletedTask;
        return _next(context);
    }
}
