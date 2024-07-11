using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.TestConstants;

public static partial class Constants
{
    public static class Users
    {
        public static readonly UserId Id = UserId.Create();

        public static readonly EmailAddress Email = EmailAddress.From("alice@example.com");

        public static readonly string UserName = "alice";

        public static readonly string FirstName = "Alice";

        public static readonly string LastName = "Doe";

        public static readonly string Password = "Password4!";
    }
}
