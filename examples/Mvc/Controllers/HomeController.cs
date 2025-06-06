using Mvc.Views;
using Pipegram.Controllers.CallbackQueries;
using Pipegram.Controllers.Messages;
using Pipegram.Routing;

namespace Mvc.Controllers;

public class HomeController : CallbackQueryControllerBase
{
    [StartAction]
    [CallbackQueryAction(Endpoints.Home.Start)]
    public static IResult Start() => new HomeStartView();

    [CallbackQueryAction(Endpoints.Home.About)]
    public static IResult About() => new HomeAboutView();

    [CallbackQueryAction(Endpoints.Home.Contact)]
    public static IResult Contact() => new HomeContactView();
}
