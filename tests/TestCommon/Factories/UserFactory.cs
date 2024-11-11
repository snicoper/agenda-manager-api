using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Users.Authentication;
using AgendaManager.TestCommon.Seeds;

namespace AgendaManager.TestCommon.Factories;

public abstract class UserFactory
{
    public static BcryptPasswordHasher BcryptPasswordHasher => new();

    public static User CreateUser(
        UserId? id = null,
        EmailAddress? email = null,
        PasswordHash? passwordHash = null,
        string? firstName = null,
        string? lastName = null,
        bool isActive = true,
        bool emailConfirmed = false)
    {
        var user = User.Create(
            userId: id ?? Users.UserAlice.Id,
            email: email ?? Users.UserAlice.Email,
            passwordHash: passwordHash ?? PasswordHash.FromHashed(BcryptPasswordHasher.HashPassword("Password4!")),
            firstName: firstName ?? Users.UserAlice.FirstName,
            lastName: lastName ?? Users.UserAlice.LastName,
            isActive: isActive,
            emailConfirmed: emailConfirmed);

        return user;
    }

    public static User CreateUserAlice()
    {
        var user = CreateUser(
            id: Users.UserAlice.Id,
            email: Users.UserAlice.Email,
            passwordHash: PasswordHash.FromHashed(BcryptPasswordHasher.HashPassword(Users.UserAlice.RawPassword)),
            firstName: Users.UserAlice.FirstName,
            lastName: Users.UserAlice.LastName);

        return user;
    }

    public static User CreateUserBob()
    {
        var user = CreateUser(
            id: Users.UserBob.Id,
            email: Users.UserBob.Email,
            passwordHash: PasswordHash.FromHashed(BcryptPasswordHasher.HashPassword(Users.UserBob.RawPassword)),
            firstName: Users.UserBob.FirstName,
            lastName: Users.UserBob.LastName);

        return user;
    }

    public static User CreateUserCarol()
    {
        var user = CreateUser(
            id: Users.UserCarol.Id,
            email: Users.UserCarol.Email,
            passwordHash: PasswordHash.FromHashed(BcryptPasswordHasher.HashPassword(Users.UserCarol.RawPassword)),
            firstName: Users.UserCarol.FirstName,
            lastName: Users.UserCarol.LastName);

        return user;
    }

    public static User CreateUserLexi()
    {
        var user = CreateUser(
            id: Users.UserLexi.Id,
            email: Users.UserLexi.Email,
            passwordHash: PasswordHash.FromHashed(BcryptPasswordHasher.HashPassword(Users.UserLexi.RawPassword)),
            firstName: Users.UserLexi.FirstName,
            lastName: Users.UserLexi.LastName);

        return user;
    }
}
