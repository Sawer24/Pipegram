using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public class MessageInterceptor : IMessageInterceptor
{
    private readonly Dictionary<long, Interceptor> _accessors = [];
    private readonly Dictionary<object, long> _ids = [];

    public object SetInterceptor(long chatId, Func<Message, bool> onMessage,
        Func<CallbackQuery, bool>? onCallbackQuery = null,
        Action? onInterrupt = null)
    {
        var interceptor = new Interceptor(onMessage, onCallbackQuery, onInterrupt);
        lock (_accessors)
        {
            if (_accessors.TryGetValue(chatId, out var actualInterceptor))
                actualInterceptor.Interrupt?.Invoke();
            _accessors[chatId] = interceptor;
            _ids[interceptor] = chatId;
        }
        return interceptor;
    }

    public bool CancelInterceptor(object key)
    {
        lock (_accessors)
        {
            if (!_ids.TryGetValue(key, out var chatId))
                return false;

            _accessors.Remove(chatId);
            _ids.Remove(key);
            return true;
        }
    }

    public bool TryUseInterceptor(UpdateContext context)
    {
        if (context.Update.Message is { } message)
        {
            lock (_accessors)
            {
                if (!_accessors.TryGetValue(message.Chat.Id, out var interceptor)
                    || !interceptor.Message(message))
                    return false;
                _accessors.Remove(message.Chat.Id);
                _ids.Remove(interceptor);
                return true;
            }
        }
        if (!(context.Update.CallbackQuery is { Message: Message callbackQueryMessage } callbackQuery))
            return false;

        lock (_accessors)
        {
            if (!_accessors.TryGetValue(callbackQueryMessage.Chat.Id, out var interceptor) ||
                (interceptor.CallbackQuery?.Invoke(callbackQuery)) != true)
                return false;
            _accessors.Remove(callbackQueryMessage.Chat.Id);
            _ids.Remove(interceptor);
            return true;
        }
    }

    private class Interceptor(Func<Message, bool> onMessage,
        Func<CallbackQuery, bool>? onCallbackQuery = null,
        Action? onInterrupt = null)
    {
        public Func<Message, bool> Message { get; } = onMessage;
        public Func<CallbackQuery, bool>? CallbackQuery { get; } = onCallbackQuery;
        public Action? Interrupt { get; } = onInterrupt;
    }
}
