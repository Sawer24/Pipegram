using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public interface ICallbackQueryInterceptor
{
    public object SetInterceptor(long chatId, int messageId, Func<CallbackQuery, bool> onCallbackQuery);
    public bool CancelInterceptor(object key);
    public bool TryUseInterceptor(UpdateContext context);
}
