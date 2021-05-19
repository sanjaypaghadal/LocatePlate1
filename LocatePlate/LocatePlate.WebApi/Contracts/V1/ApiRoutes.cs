namespace LocatePlate.WebApi.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version1 = "v1";
        public const string Base = Root + "/" + Version1;
        public static class BaseRoute
        {
            public const string V1 = Base + "/[controller]";
        }

    }
}
