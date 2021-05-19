namespace LocatePlate.WebApi.Contracts.Mvc
{
    public static class AdminRoutes
    {
        public const string Root = "Admin";
        public static class BaseRoute
        {
            public const string Admin = Root + "/[controller]";
        }

    }
}
