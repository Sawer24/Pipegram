using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers;

public abstract class MessageControllerBase : TelegramControllerBase
{
    public Message Message => Update.Message
        ?? throw new InvalidOperationException("Update.Message is null.");

    public Task SendMessage(string text, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
        => Client.SendMessage(Message.Chat.Id, text, parseMode, replyMarkup: replyMarkup);
}
