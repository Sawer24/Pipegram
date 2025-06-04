namespace Pipegram.Routing;

public class NothingResult : IResult
{
    public static IResult Instance { get; } = new NothingResult();
    
    private NothingResult() { }

    public Task Execute(UpdateContext context)
    {
        // No operation, just return completed task
        return Task.CompletedTask;
    }
}
