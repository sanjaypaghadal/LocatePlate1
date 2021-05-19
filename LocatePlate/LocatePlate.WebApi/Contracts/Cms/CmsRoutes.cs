namespace LocatePlate.WebApi.Contracts.CmsRoutes
{
    public static class CmsRoute
    {
        public const string Root = "Cms";
        public static class BaseRoute
        {
            public const string Cms = Root + "/[controller]";
        }

    }
}
