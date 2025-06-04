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
        var client = context.TelegramBot.Client
            ?? throw new ArgumentException("TelegramBot.Client is null.", nameof(context));

        var text = this.text.ToString();
        if (string.IsNullOrEmpty(text))
            throw new InvalidOperationException("View text cannot be null or empty.");

        switch (context.Update.Type)
        {
            case UpdateType.Message:
                return client.SendMessage(context.Update.Message!.Chat, text, parseMode, replyMarkup: keyboard);

            case UpdateType.CallbackQuery:
                if (context.Update.CallbackQuery!.Message is Message message)
                    return client.EditMessageText(message.Chat, message.Id, text, parseMode, replyMarkup: keyboard);
                return client.SendMessage(context.Update.CallbackQuery.From.Id, text, parseMode, replyMarkup: keyboard);

            default:
                throw new ArgumentException($"Unsupported update type: {context.Update.Type}", nameof(context));
        }
    }
}
