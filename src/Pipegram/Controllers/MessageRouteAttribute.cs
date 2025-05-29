namespace Pipegram.Controllers;

[AttributeUsage(AttributeTargets.Class)]
public class MessageRouteAttribute(string route) : RouteAttribute(route)
{
}
