namespace Pipegram;

/// <summary>
/// Extension methods for adding middleware.
/// </summary>
public static class Middleware_PipelineBuilderExtensions
{
    /// <summary>
    /// Adds a middleware delegate defined in-line to the bot's update pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IPipelineBuilder"/> instance.</param>
    /// <param name="middleware">A function that handles the update and calls the given next function.</param>
    /// <returns>The <see cref="IPipelineBuilder"/> instance.</returns>
    public static IPipelineBuilder Use(this IPipelineBuilder builder, Func<UpdateContext, Func<Task>, Task> middleware)
        => builder.Use(next => context => middleware(context, () => next(context)));

    /// <summary>
    /// Adds a middleware delegate defined in-line to the bot's update pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IPipelineBuilder"/> instance.</param>
    /// <param name="middleware">A function that handles the update and calls the given next function.</param>
    /// <returns>The <see cref="IPipelineBuilder"/> instance.</returns>
    public static IPipelineBuilder Use(this IPipelineBuilder builder, Func<UpdateContext, UpdateDelegate, Task> middleware)
        => builder.Use(next => context => middleware(context, next));
}
