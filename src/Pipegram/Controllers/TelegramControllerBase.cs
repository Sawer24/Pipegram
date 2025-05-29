using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers;

public abstract class TelegramControllerBase
{
    private UpdateContext? _context;

    private UpdateContext Context => _context
        ?? throw new InvalidOperationException("TelegramControllerBase not initialized. Call Initialize method first.");

    public ITelegramBot Bot => Context.TelegramBot;
    public ITelegramBotClient Client => Bot.Client
        ?? throw new InvalidOperationException("TelegramBot not initialized. Call RunAsync method first.");
    public User BotUser => Bot.BotUser
        ?? throw new InvalidOperationException("TelegramBot not initialized. Call RunAsync method first.");

    public Update Update => Context.Update;
    public IServiceProvider ServiceProvider => Context.Services;

    public TelegramControllerBase Initialize(UpdateContext context)
    {
        _context = context;
        Initialize();
        return this;
    }

    protected virtual void Initialize()
    {
    }

    public static InlineKeyboardMarkup InlineKeyboardMarkup(params InlineKeyboardButton[] buttons) => new(buttons);

    public static InlineKeyboardMarkup InlineKeyboardMarkup(params InlineKeyboardButton[][] rows) => new(rows);

    public static InlineKeyboardButton CallbackButton(string text, string action, params string[] args)
        => InlineKeyboardButton.WithCallbackData(text, args.Length == 0 ? action : action + ' ' + string.Join(' ', args));
}
