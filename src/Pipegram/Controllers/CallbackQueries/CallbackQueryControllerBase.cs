using Microsoft.Extensions.DependencyInjection;
using Pipegram.Interceptions;
using Pipegram.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers.CallbackQueries;

public abstract class CallbackQueryControllerBase : TelegramControllerBase
{
    public CallbackQuery CallbackQuery => Update.CallbackQuery
        ?? throw new InvalidOperationException("Update.CallbackQuery is null.");
    public Message Message => CallbackQuery.Message
        ?? throw new InvalidOperationException("Update.CallbackQuery.Message is null.");

    public async Task<NothingResult> SendMessage(string text, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        await BotClient.SendMessage(Message.Chat.Id, text, parseMode, replyMarkup: replyMarkup);
        return NothingResult.Instance;
    }

    public async Task<NothingResult> EditMessage(string? text = null, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        await BotClient.EditMessageText(Message.Chat, Message.Id,
            text ?? Message.Text ?? string.Empty, parseMode, replyMarkup: replyMarkup);
        return NothingResult.Instance;
    }

    public async Task<NothingResult> DeleteMessage()
    {
        await BotClient.DeleteMessage(Message.Chat.Id, Message.Id);
        return NothingResult.Instance;
    }

    public async Task<NothingResult> DeleteMessage(int messageId)
    {
        await BotClient.DeleteMessage(Message.Chat.Id, messageId);
        return NothingResult.Instance;
    }

    public async Task<IMessageInterceptResult> InterceptMessage(int? timeoutInMilliseconds = 60 * 60 * 1000, bool deleteAfterIntercept = false)
    {
        var messageInterceptor = Services.GetRequiredService<IMessageInterceptor>();
        var result = await messageInterceptor.InterceptMessage(Message.Chat.Id, Message.Id, timeoutInMilliseconds);
        if (deleteAfterIntercept && result.IsMessage)
            await DeleteMessage(result.Message.Id);
        return result;
    }

    public Task<ICallbackQueryInterceptResult> InterceptCallbackQuery(int? timeoutInMilliseconds = 60 * 60 * 1000)
    {
        var callbackQueryInterceptor = Services.GetRequiredService<ICallbackQueryInterceptor>();
        return callbackQueryInterceptor.InterceptCallbackQuery(Message.Chat.Id, Message.Id, timeoutInMilliseconds);
    }
}
