using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

namespace Pipegram.Interceptions;

public interface ICallbackQueryInterceptResult
{
    [MemberNotNullWhen(true, nameof(CallbackQuery))]
    bool IsCallbackQuery { get; }
    bool IsTimeout { get; }
    CallbackQuery? CallbackQuery { get; }
}
