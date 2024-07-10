namespace AgendaManager.Domain.Common.Constants;

public static class Permissions
{
    public static class Authorization
    {
        public const string Create = "authorization:create";
        public const string Read = "authorization:read";
        public const string Update = "authorization:update";
        public const string Delete = "authorization:delete";
    }

    public static class User
    {
        public const string Create = "user.create";
        public const string Read = "user.read";
        public const string Update = "user.update";
        public const string Delete = "user.delete";
    }
}
