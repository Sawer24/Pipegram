using Mvc.Views;
using Pipegram.Controllers.CallbackQueries;
using Pipegram.Routing;

namespace Mvc.Controllers;

public class HomeController : CallbackQueryControllerBase
{
    [CallbackQueryAction(Endpoints.Home.Menu)]
    public static IResult Index() => new HomeMenuView();

    [CallbackQueryAction(Endpoints.Home.About)]
    public static IResult About() => new HomeAboutView();

    [CallbackQueryAction(Endpoints.Home.Contact)]
    public static IResult Contact() => new HomeContactView();
}
