namespace Mvc;

public static class Endpoints
{
    public const string VersionBase = "v1";

    public static class Home
    {
        public const string Base = VersionBase + "/home";

        public const string Start = Base + "/start";
        public const string About = Base + "/about";
        public const string Contact = Base + "/contact";
    }

    public static class Items
    {
        public const string Base = VersionBase + "/items";

        public const string List = Base + "/list";
        public const string Item = Base + "/item";
        public const string Create = Base + "/create";
        public const string Edit = Base + "/edit";
        public const string Delete = Base + "/delete";
    }
}
