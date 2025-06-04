namespace Pipegram.Routing;

public class FromFunctionResult(UpdateDelegate func) : IResult
{
    public Task Execute(UpdateContext context) => func(context);
}
