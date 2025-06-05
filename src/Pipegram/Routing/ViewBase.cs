using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Routing;

public abstract class ViewBase : IResult
{
    protected StringBuilder text = new();
    protected ParseMode parseMode = ParseMode.None;
    protected InlineKeyboardMarkup keyboard = new();

    public Task Execute(UpdateContext context)
    {
        var client = context.BotClient
            ?? throw new ArgumentException("TelegramBot.Client is null.", nameof(context));

        var text = this.text.ToString();
        if (string.IsNullOrEmpty(text))
            throw new InvalidOperationException("View text cannot be null or empty.");

        if (context.Update.Message is Message message)
            return client.SendMessage(message.Chat, text, parseMode, replyMarkup: keyboard);

        if (context.Update.CallbackQuery is CallbackQuery callbackQuery)
        {
            if (callbackQuery.Message is Message callbackMessage)
                return client.EditMessageText(callbackMessage.Chat, callbackMessage.Id, text, parseMode, replyMarkup: keyboard);
            return client.SendMessage(context.Update.CallbackQuery.From.Id, text, parseMode, replyMarkup: keyboard);
        }
        throw new ArgumentException($"Unsupported update type: {context.Update.Type}", nameof(context));
    }
}
