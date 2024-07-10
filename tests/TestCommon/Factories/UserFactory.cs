using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Authentication;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static PasswordHasher PasswordHasher => new();

    public static User CreateUser()
    {
        var passwordHash = PasswordHasher.HashPassword(Constants.Users.Password).Value ??
            throw new InvalidOperationException("Failed to hash password");

        return User.Create(
            Constants.Users.Id,
            Constants.Users.Email,
            Constants.Users.UserName,
            passwordHash,
            Constants.Users.FirstName,
            Constants.Users.LastName);
    }
}
