using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Seeds;

public static class Users
{
    public static class UserAlice
    {
        public static readonly UserId Id = UserId.Create();

        public static readonly EmailAddress Email = EmailAddress.From("alice@example.com");

        public static readonly string FirstName = "Alice";

        public static readonly string LastName = "Doe";

        public static readonly string RawPassword = "Password4!";
    }

    public static class UserBob
    {
        public static readonly UserId Id = UserId.Create();

        public static readonly EmailAddress Email = EmailAddress.From("bob@example.com");

        public static readonly string FirstName = "Bob";

        public static readonly string LastName = "Doe";

        public static readonly string RawPassword = "Password4!";
    }

    public static class UserCarol
    {
        public static readonly UserId Id = UserId.Create();

        public static readonly EmailAddress Email = EmailAddress.From("carol@example.com");

        public static readonly string FirstName = "Carol";

        public static readonly string LastName = "Doe";

        public static readonly string RawPassword = "Password4!";
    }

    public static class UserLexi
    {
        public static readonly UserId Id = UserId.Create();

        public static readonly EmailAddress Email = EmailAddress.From("lexi@example.com");

        public static readonly string FirstName = "Lexi";

        public static readonly string LastName = "Doe";

        public static readonly string RawPassword = "Password4!";
    }
}
