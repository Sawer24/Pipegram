using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public class CallbackQueryInterceptResult : ICallbackQueryInterceptResult
{
    private bool _isCompleted;

    public bool IsCompleted { get => _isCompleted; private set => _isCompleted = value; }

    public CallbackQuery? CallbackQuery { get; private set; }
    [MemberNotNullWhen(true, nameof(CallbackQuery))]
    public bool IsCallbackQuery => CallbackQuery is not null;
    public bool IsTimeout { get; private set; }

    private bool TryComplete()
        => !Interlocked.CompareExchange(ref _isCompleted, true, _isCompleted);

    public void TrySetCallbackQuery(CallbackQuery callbackQuery)
    {
        if (TryComplete())
            CallbackQuery = callbackQuery;
    }
    public void TrySetTimeout()
    {
        if (TryComplete())
            IsTimeout = true;
    }
}
