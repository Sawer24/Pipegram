namespace Pipegram.Controllers;

[AttributeUsage(AttributeTargets.Class)]
public class CallbackQueryRouteAttribute(string route) : RouteAttribute(route)
{
}
