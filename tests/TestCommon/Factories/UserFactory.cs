using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Users;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static BcryptPasswordHasher BcryptPasswordHasher => new();

    public static User CreateUserAlice()
    {
        var passwordHash = BcryptPasswordHasher.HashPassword(Constants.UserAlice.RawPassword) ??
            throw new InvalidOperationException("Failed to hash password");

        return User.Create(
            Constants.UserAlice.Id,
            Constants.UserAlice.Email,
            Constants.UserAlice.UserName,
            passwordHash,
            Constants.UserAlice.FirstName,
            Constants.UserAlice.LastName);
    }

    public static User CreateUserBob()
    {
        var passwordHash = BcryptPasswordHasher.HashPassword(Constants.UserBob.RawPassword) ??
            throw new InvalidOperationException("Failed to hash password");

        return User.Create(
            Constants.UserBob.Id,
            Constants.UserBob.Email,
            Constants.UserBob.UserName,
            passwordHash,
            Constants.UserBob.FirstName,
            Constants.UserBob.LastName);
    }

    public static User CreateUserCarol()
    {
        var passwordHash = BcryptPasswordHasher.HashPassword(Constants.UserCarol.RawPassword) ??
            throw new InvalidOperationException("Failed to hash password");

        return User.Create(
            Constants.UserCarol.Id,
            Constants.UserCarol.Email,
            Constants.UserCarol.UserName,
            passwordHash,
            Constants.UserCarol.FirstName,
            Constants.UserCarol.LastName);
    }

    public static User CreateUserLexi()
    {
        var passwordHash = BcryptPasswordHasher.HashPassword(Constants.UserLexi.RawPassword) ??
            throw new InvalidOperationException("Failed to hash password");

        return User.Create(
            Constants.UserLexi.Id,
            Constants.UserLexi.Email,
            Constants.UserLexi.UserName,
            passwordHash,
            Constants.UserLexi.FirstName,
            Constants.UserLexi.LastName);
    }
}
