using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Users;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static BcryptPasswordHasher BcryptPasswordHasher => new();

    public static User CreateUser(
        UserId? id = null,
        EmailAddress? email = null,
        string? userName = null,
        string? passwordHash = null,
        string? firstName = null,
        string? lastName = null)
    {
        return User.Create(
            id ?? Constants.UserAlice.Id,
            email ?? Constants.UserAlice.Email,
            userName ?? Constants.UserAlice.UserName,
            passwordHash ?? BcryptPasswordHasher.HashPassword("Password4!"),
            firstName ?? Constants.UserAlice.FirstName,
            lastName ?? Constants.UserAlice.LastName);
    }

    public static User CreateUserAlice()
    {
        return CreateUser(
            Constants.UserAlice.Id,
            Constants.UserAlice.Email,
            Constants.UserAlice.UserName,
            Constants.UserAlice.RawPassword,
            Constants.UserAlice.FirstName,
            Constants.UserAlice.LastName);
    }

    public static User CreateUserBob()
    {
        return CreateUser(
            Constants.UserBob.Id,
            Constants.UserBob.Email,
            Constants.UserBob.UserName,
            Constants.UserBob.RawPassword,
            Constants.UserBob.FirstName,
            Constants.UserBob.LastName);
    }

    public static User CreateUserCarol()
    {
        return CreateUser(
            Constants.UserCarol.Id,
            Constants.UserCarol.Email,
            Constants.UserCarol.UserName,
            Constants.UserCarol.RawPassword,
            Constants.UserCarol.FirstName,
            Constants.UserCarol.LastName);
    }

    public static User CreateUserLexi()
    {
        return CreateUser(
            Constants.UserLexi.Id,
            Constants.UserLexi.Email,
            Constants.UserLexi.UserName,
            Constants.UserLexi.RawPassword,
            Constants.UserLexi.FirstName,
            Constants.UserLexi.LastName);
    }
}
