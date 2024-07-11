namespace AgendaManager.WebApi.UnitTests;

public static class Enpoints
{
    public const string BaseUri = "api/v1";

    public static class Authentication
    {
        public const string Login = $"{BaseUri}/authentication/login";
    }
}
