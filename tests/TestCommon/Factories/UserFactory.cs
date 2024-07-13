using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Users;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static BcryptPasswordHasher BcryptPasswordHasher => new();

    public static User CreateUser()
    {
        var passwordHash = BcryptPasswordHasher.HashPassword(Constants.Users.Password) ??
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
