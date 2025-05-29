using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace Pipegram.Routing;

public static class Results
{
    public static IResult SendMessage(string text, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
        => FromAction(context
            => context.TelegramBot.Client!.SendMessage(context.Update.Message!.Chat.Id, text, parseMode, replyMarkup: replyMarkup));

    private static ActionResult FromAction(Func<UpdateContext, Task> action) => new(action);

    private class ActionResult(Func<UpdateContext, Task> action) : IResult
    {
        public Task Execute(UpdateContext context) => action(context);
    }
}