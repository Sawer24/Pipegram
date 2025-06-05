using Microsoft.Extensions.DependencyInjection;
using Pipegram.Interceptions;
using Pipegram.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers.Messages;

public abstract class MessageControllerBase : TelegramControllerBase
{
    public Message Message => Update.Message
        ?? throw new InvalidOperationException("Update.Message is null.");

    public async Task<NothingResult> SendMessage(string text, ParseMode parseMode = ParseMode.None,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        await BotClient.SendMessage(Message.Chat.Id, text, parseMode, replyMarkup: replyMarkup);
        return NothingResult.Instance;
    }

    public async Task<NothingResult> DeleteMessage(int messageId)
    {
        await BotClient.DeleteMessage(Message.Chat.Id, messageId);
        return NothingResult.Instance;
    }

    public async Task<IMessageInterceptResult> InterceptMessage(int? messageId = null, int timeoutInMilliseconds = 60 * 60 * 1000,
        bool deleteAfterIntercept = false)
    {
        var messageInterceptor = Services.GetRequiredService<IMessageInterceptor>();
        var message = await messageInterceptor.InterceptMessage(Message.Chat.Id, messageId, timeoutInMilliseconds);
        if (deleteAfterIntercept && message.IsMessage)
            await DeleteMessage(message.Message.Id);
        return message;
    }
}
