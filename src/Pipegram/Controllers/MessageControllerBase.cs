using Microsoft.Extensions.DependencyInjection;
using Pipegram.Interceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers;

public abstract class MessageControllerBase : TelegramControllerBase
{
    private IMessageInterceptor? _messageInterceptor;

    public Message Message => Update.Message
        ?? throw new InvalidOperationException("Update.Message is null.");

    public Task SendMessage(string text, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
        => Client.SendMessage(Message.Chat.Id, text, parseMode, replyMarkup: replyMarkup);

    public Task DeleteMessage(int messageId) => Client.DeleteMessage(Message.Chat.Id, messageId);

    public async Task<Message?> InterceptMessage(int timeoutInMilliseconds = 60 * 60 * 1000, bool deleteAfterIntercept = false)
    {
        _messageInterceptor ??= ServiceProvider.GetRequiredService<IMessageInterceptor>();
        var message = await _messageInterceptor.InterceptMessage(Message.Chat.Id, timeoutInMilliseconds);
        if (deleteAfterIntercept && message != null)
            await DeleteMessage(message.Id);
        return message;
    }
}
