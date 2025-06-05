using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace Pipegram.Routing;

public class SendMessageResult(string text, ParseMode parseMode = ParseMode.None, InlineKeyboardMarkup? replyMarkup = null) : IResult
{
    private readonly string _text = text;
    private readonly ParseMode _parseMode = parseMode;
    private readonly InlineKeyboardMarkup? _replyMarkup = replyMarkup;

    public Task Execute(UpdateContext context)
    {
        var client = context.BotClient
            ?? throw new ArgumentException("TelegramBot.Client is null.", nameof(context));

        var chatId = context.Update.Message?.Chat.Id
            ?? context.Update.CallbackQuery?.Message?.Chat.Id
            ?? throw new ArgumentException("Update does not contain a valid chat ID.", nameof(context));

        return client.SendMessage(chatId, _text, _parseMode, replyMarkup: _replyMarkup);
    }
}
