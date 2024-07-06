﻿using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.TestConstants;

public static partial class Constants
{
    public static class Users
    {
        public static readonly UserId Id = UserId.Create();

        public static readonly Email Email = Email.From("test@example.com");

        public static readonly string UserName = "test";

        public static readonly string Password = "<PASSWORD>";

        public static readonly string FirstName = "test";

        public static readonly string LastName = "test";
    }
}
