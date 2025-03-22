namespace AgendaManager.WebApi.UnitTests;

public static class Enpoints
{
    public const string BaseUri = "api/v1";

    public static class Authentication
    {
        public const string Login = $"{BaseUri}/auth/login";
    }

    public static class UsersAccounts
    {
        public const string RequestPasswordReset = $"{BaseUri}/accounts/request-password-reset";
    }
}
