using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Routing;

/// <summary>A factory for <see cref="IResult"/>.</summary>
public static class Results
{
    public static IResult Nothing() => NothingResult.Instance;

    public static IResult FromFunction(UpdateDelegate func) => new FromFunctionResult(func);

    public static IResult SendMessage(string text, ParseMode parseMode = ParseMode.None, InlineKeyboardMarkup? replyMarkup = null)
        => new SendMessageResult(text, parseMode, replyMarkup);
}
