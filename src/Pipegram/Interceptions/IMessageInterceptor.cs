using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public interface IMessageInterceptor
{
    public object SetInterceptor(long chatId, Func<Message, bool> onMessage,
        Func<CallbackQuery, bool>? onCallbackQuery = null,
        Action? onInterrupt = null);
    public bool CancelInterceptor(object key);
    public bool TryUseInterceptor(UpdateContext context);
}
