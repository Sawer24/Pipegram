using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public class MessageInterceptResult : IMessageInterceptResult
{
    private bool _isCompleted;

    public bool IsCompleted { get => _isCompleted; private set => _isCompleted = value; }

    public Message? Message { get; private set; }
    public CallbackQuery? CallbackQuery { get; private set; }

    [MemberNotNullWhen(true, nameof(Message))]
    public bool IsMessage => Message is not null;
    [MemberNotNullWhen(true, nameof(CallbackQuery))]
    public bool IsCallbackQuery => CallbackQuery is not null;
    public bool IsTimeout { get; private set; }
    public bool IsInterrupted { get; private set; }

    private bool TryComplete()
        => !Interlocked.CompareExchange(ref _isCompleted, true, _isCompleted);

    public void TrySetMessage(Message message)
    {
        if (TryComplete())
            Message = message;
    }

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

    public void TrySetInterrupted()
    {
        if (TryComplete())
            IsInterrupted = true;
    }

    public static implicit operator Message(MessageInterceptResult result)
    {
        if (result is null || !result.IsMessage)
            throw new InvalidCastException("Cannot cast MessageInterceptResult to Message, because it does not contain a message.");
        return result.Message!;
    }

    public static implicit operator CallbackQuery(MessageInterceptResult result)
    {
        if (result is null || !result.IsCallbackQuery)
            throw new InvalidCastException("Cannot cast MessageInterceptResult to CallbackQuery, because it does not contain a callback query.");
        return result.CallbackQuery!;
    }
}
