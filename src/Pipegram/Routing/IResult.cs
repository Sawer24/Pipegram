namespace Pipegram.Routing;

public interface IResult
{
    Task Execute(UpdateContext context);
}
