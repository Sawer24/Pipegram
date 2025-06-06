using Pipegram.Routing;

namespace Mvc.Views;

public class HomeAboutView : ViewBase
{
    public HomeAboutView()
    {
        text.Append("This is the Pipegram MVC example application.");
        keyboard.AddNewRow(("Back to menu", Endpoints.Home.Start));
    }
}
