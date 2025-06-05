namespace Pipegram;

public interface IPipelineBuilder
{
    IPipelineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware);

    UpdateDelegate Build();
}
