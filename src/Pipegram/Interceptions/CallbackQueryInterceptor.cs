using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public class CallbackQueryInterceptor : ICallbackQueryInterceptor
{
    private readonly Dictionary<(long chatId, int messageId), Interceptor> _accessors = [];
    private readonly Dictionary<object, (long chatId, int messageId)> _ids = [];

    public object SetInterceptor(long chatId, int messageId, Func<CallbackQuery, bool> onCallbackQuery)
    {
        var interceptor = new Interceptor(onCallbackQuery);
        lock (_accessors)
        {
            if (_accessors.ContainsKey((chatId, messageId)))
                throw new InvalidOperationException($"Interceptor for chatId {chatId} and messageId {messageId} already exists.");
            _accessors[(chatId, messageId)] = interceptor;
            _ids[interceptor] = (chatId, messageId);
        }
        return interceptor;
    }

    public bool CancelInterceptor(object key)
    {
        lock (_accessors)
        {
            if (!_ids.TryGetValue(key, out var idPair))
                return false;

            _accessors.Remove(idPair);
            _ids.Remove(key);
            return true;
        }
    }

    public bool TryUseInterceptor(UpdateContext context)
    {
        if (!(context.Update.CallbackQuery?.Message is { } message))
            return false;

        lock (_accessors)
        {
            if (!_accessors.TryGetValue((message.Chat.Id, message.Id), out var interceptor)
                || !interceptor.CallbackQuery(context.Update.CallbackQuery))
                return false;

            _accessors.Remove((message.Chat.Id, message.Id));
            _ids.Remove(interceptor);
            return true;
        }
    }

    private class Interceptor(Func<CallbackQuery, bool> onCallbackQuery)
    {
        public Func<CallbackQuery, bool> CallbackQuery { get; } = onCallbackQuery;
    }
}
