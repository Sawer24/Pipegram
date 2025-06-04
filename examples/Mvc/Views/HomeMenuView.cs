using Pipegram.Routing;

namespace Mvc.Views;

public class HomeMenuView : ViewBase
{
    public HomeMenuView()
    {
        text.Append("Welcome to the Pipegram MVC example!");
        keyboard.AddNewRow(("Items list", Endpoints.Items.List));
        keyboard.AddNewRow(("About", Endpoints.Home.About), ("Contact", Endpoints.Home.Contact));
    }
}
