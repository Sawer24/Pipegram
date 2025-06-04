using Pipegram.Routing;

namespace Mvc.Views;

public class HomeContactView : ViewBase
{
    public HomeContactView()
    {
        text.Append("Contacts");
        keyboard.AddNewRow(("Back to menu", Endpoints.Home.Menu));
    }
}
