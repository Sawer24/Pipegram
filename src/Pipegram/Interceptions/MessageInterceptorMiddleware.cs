using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public class MessageInterceptorMiddleware(UpdateDelegate next, IMessageInterceptor messageInterceptor)
{
    private readonly UpdateDelegate _next = next;
    private readonly IMessageInterceptor _messageInterceptor = messageInterceptor;

    public Task Invoke(UpdateContext context)
    {
        if (context.Update.Type != Telegram.Bot.Types.Enums.UpdateType.Message
            || context.Update.Message is not Message message)
            return _next(context);
        var interceptor = _messageInterceptor.UseInterceptor(message.Chat.Id);
        if (interceptor is not null)
            return interceptor(context);
        return _next(context);
    }
}
