using Microsoft.Extensions.DependencyInjection;
using Pipegram.Interceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers;

public abstract class CallbackQueryControllerBase : TelegramControllerBase
{
    public CallbackQuery CallbackQuery => Update.CallbackQuery
        ?? throw new InvalidOperationException("Update.CallbackQuery is null.");
    public Message Message => CallbackQuery.Message
        ?? throw new InvalidOperationException("Update.CallbackQuery.Message is null.");

    public Task SendMessage(string text, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
        => Client.SendMessage(Message.Chat.Id, text, parseMode, replyMarkup: replyMarkup);

    public Task EditMessage(string? text = null, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
        => Client.EditMessageText(Message.Chat, Message.Id,
            text ?? Message.Text ?? string.Empty, parseMode, replyMarkup: replyMarkup);

    public Task DeleteMessage() => Client.DeleteMessage(Message.Chat.Id, Message.Id);
    public Task DeleteMessage(int messageId) => Client.DeleteMessage(Message.Chat.Id, messageId);

    public async Task<IMessageInterceptResult> InterceptMessage(int? timeoutInMilliseconds = 60 * 60 * 1000, bool deleteAfterIntercept = false)
    {
        var messageInterceptor = ServiceProvider.GetRequiredService<IMessageInterceptor>();
        var result = await messageInterceptor.InterceptMessage(Message.Chat.Id, Message.Id, timeoutInMilliseconds);
        if (deleteAfterIntercept && result.IsMessage)
            await DeleteMessage(result.Message.Id);
        return result;
    }

    public Task<ICallbackQueryInterceptResult> InterceptCallbackQuery(int? timeoutInMilliseconds = 60 * 60 * 1000)
    {
        var callbackQueryInterceptor = ServiceProvider.GetRequiredService<ICallbackQueryInterceptor>();
        return callbackQueryInterceptor.InterceptCallbackQuery(Message.Chat.Id, Message.Id, timeoutInMilliseconds);
    }
}
