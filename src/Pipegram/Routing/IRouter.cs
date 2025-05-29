namespace Pipegram.Routing;

public interface IRouter
{
    Task Match(UpdateContext context);
}
