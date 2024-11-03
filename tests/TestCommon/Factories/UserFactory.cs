using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Users.Authentication;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static BcryptPasswordHasher BcryptPasswordHasher => new();

    public static User CreateUser(
        UserId? id = null,
        EmailAddress? email = null,
        string? passwordHash = null,
        string? firstName = null,
        string? lastName = null)
    {
        return User.Create(
            id ?? Constants.UserAlice.Id,
            email ?? Constants.UserAlice.Email,
            passwordHash ?? BcryptPasswordHasher.HashPassword("Password4!"),
            firstName ?? Constants.UserAlice.FirstName,
            lastName ?? Constants.UserAlice.LastName);
    }

    public static User CreateUserAlice()
    {
        return CreateUser(
            id: Constants.UserAlice.Id,
            email: Constants.UserAlice.Email,
            passwordHash: Constants.UserAlice.RawPassword,
            firstName: Constants.UserAlice.FirstName,
            lastName: Constants.UserAlice.LastName);
    }

    public static User CreateUserBob()
    {
        return CreateUser(
            id: Constants.UserBob.Id,
            email: Constants.UserBob.Email,
            passwordHash: Constants.UserBob.RawPassword,
            firstName: Constants.UserBob.FirstName,
            lastName: Constants.UserBob.LastName);
    }

    public static User CreateUserCarol()
    {
        return CreateUser(
            id: Constants.UserCarol.Id,
            email: Constants.UserCarol.Email,
            passwordHash: Constants.UserCarol.RawPassword,
            firstName: Constants.UserCarol.FirstName,
            lastName: Constants.UserCarol.LastName);
    }

    public static User CreateUserLexi()
    {
        return CreateUser(
            id: Constants.UserLexi.Id,
            email: Constants.UserLexi.Email,
            passwordHash: Constants.UserLexi.RawPassword,
            firstName: Constants.UserLexi.FirstName,
            lastName: Constants.UserLexi.LastName);
    }
}
