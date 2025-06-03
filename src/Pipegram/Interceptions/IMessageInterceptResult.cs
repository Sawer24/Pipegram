using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public interface IMessageInterceptResult
{
    [MemberNotNullWhen(true, nameof(Message))]
    bool IsMessage { get; }
    [MemberNotNullWhen(true, nameof(CallbackQuery))]
    bool IsCallbackQuery { get; }
    bool IsTimeout { get; }
    bool IsInterrupted { get; }
    Message? Message { get; }
    CallbackQuery? CallbackQuery { get; }
}