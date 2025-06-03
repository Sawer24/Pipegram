using Microsoft.Extensions.DependencyInjection;
using Pipegram.Interceptions;
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

    public Task DeleteMessage(int messageId) => Client.DeleteMessage(Message.Chat.Id, messageId);

    public async Task<IMessageInterceptResult> InterceptMessage(int? messageId = null, int timeoutInMilliseconds = 60 * 60 * 1000,
        bool deleteAfterIntercept = false)
    {
        var messageInterceptor = ServiceProvider.GetRequiredService<IMessageInterceptor>();
        var message = await messageInterceptor.InterceptMessage(Message.Chat.Id, messageId, timeoutInMilliseconds);
        if (deleteAfterIntercept && message.IsMessage)
            await DeleteMessage(message.Message.Id);
        return message;
    }
}
