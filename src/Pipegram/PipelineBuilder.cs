namespace Pipegram;

public class PipelineBuilder : IPipelineBuilder
{
    private const string UpdateUnhandledKey = "__UpdateUnhandled";

    private readonly List<Func<UpdateDelegate, UpdateDelegate>> _components = [];

    public IPipelineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
    {
        _components.Add(middleware);
        return this;
    }

    public UpdateDelegate Build()
    {
        UpdateDelegate pipeline = context =>
        {
            context.Items[UpdateUnhandledKey] = true;
            return Task.CompletedTask;
        };

        for (var c = _components.Count - 1; c >= 0; c--)
            pipeline = _components[c](pipeline);

        return pipeline;
    }
}
