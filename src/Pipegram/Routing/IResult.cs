namespace Pipegram.Routing;

/// <summary>Represents the executable result of the endpoint execution.</summary>
public interface IResult
{
    /// <summary>Executes the result in the given context. This method will be called after the endpoint is executed.</summary>
    /// <param name="context">The <see cref="UpdateContext"/>.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    Task Execute(UpdateContext context);
}
