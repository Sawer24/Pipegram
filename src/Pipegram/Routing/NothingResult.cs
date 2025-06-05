namespace Pipegram.Routing;

/// <summary>Represents a result that does nothing.</summary>
public class NothingResult : IResult
{
    /// <summary>Singleton instance of <see cref="NothingResult"/>.</summary>
    public static NothingResult Instance { get; } = new NothingResult();
    
    private NothingResult() { }

    /// <inheritdoc/>
    public Task Execute(UpdateContext context)
    {
        // No operation, just return completed task
        return Task.CompletedTask;
    }
}
