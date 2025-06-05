using Pipegram.Routing;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Pipegram.Controllers;

public abstract class TelegramControllerBase : ITelegramInitializableController
{
    private UpdateContext? _context;

    private UpdateContext Context => _context
        ?? throw new InvalidOperationException("TelegramControllerBase not initialized. Call Initialize method first.");

    public Update Update => Context.Update;
    public ITelegramBotClient BotClient => Context.BotClient;
    public User BotUser => Context.BotUser;
    public IServiceProvider Services => Context.Services;

    public ITelegramController Initialize(UpdateContext context)
    {
        _context = context;
        Initialize();
        return this;
    }

    protected virtual void Initialize()
    {
    }

    public async Task<NothingResult> Execute(IResult result)
    {
        await result.Execute(Context);
        return NothingResult.Instance;
    }

    public async Task<NothingResult> ShowView(ViewBase view)
    {
        await view.Execute(Context);
        return NothingResult.Instance;
    }

    public static InlineKeyboardButton CallbackButton(string text, string action, params string[] args)
        => InlineKeyboardButton.WithCallbackData(text, args.Length == 0 ? action : action + ' ' + string.Join(' ', args));
}
