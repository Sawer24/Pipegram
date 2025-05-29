using Telegram.Bot.Types;

namespace Pipegram;

public class UpdateContext
{
    private Guid? _id;

    public required ITelegramBot TelegramBot { get; init; }
    public required Update Update { get; init; }
    public required IServiceProvider Services { get; init; }
    public IDictionary<object, object?> Items { get; } = new Dictionary<object, object?>();
    public required CancellationToken CancellationToken { get; init; }
    public Guid TraceIdentifier => _id ??= Guid.NewGuid();
}
