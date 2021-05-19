namespace LocatePlate.WebApi.Contracts.V2
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version2 = "v2";
        public const string Base = Root + "/" + Version2;
        public static class BaseRoute
        {
            public const string V2 = Base + "/[controller]";
        }

    }
}
