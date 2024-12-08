using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Users.Authentication;
using AgendaManager.TestCommon.Constants;

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
        bool isCollaborator = true,
        bool emailConfirmed = false)
    {
        User user = new(
            userId: id ?? UserConstants.UserAlice.Id,
            email: email ?? UserConstants.UserAlice.Email,
            passwordHash: passwordHash ?? PasswordHash.FromHashed(BcryptPasswordHasher.HashPassword("Password4!")),
            isActive: isActive,
            isCollaborator: isCollaborator,
            emailConfirmed: emailConfirmed);

        // Create user profile.
        var userProfile = UserProfile.Create(
            userProfileId: UserProfileId.Create(),
            userId: user.Id,
            firstName: firstName ?? UserConstants.UserAlice.FirstName,
            lastName: lastName ?? UserConstants.UserAlice.LastName,
            phoneNumber: null,
            address: null,
            identityDocument: null);

        user.AddProfile(userProfile);

        return user;
    }

    public static User CreateUserAlice()
    {
        var user = CreateUser(
            id: UserConstants.UserAlice.Id,
            email: UserConstants.UserAlice.Email,
            passwordHash: PasswordHash.FromHashed(
                BcryptPasswordHasher.HashPassword(UserConstants.UserAlice.RawPassword)));

        return user;
    }

    public static User CreateUserBob()
    {
        var user = CreateUser(
            id: UserConstants.UserBob.Id,
            email: UserConstants.UserBob.Email,
            passwordHash: PasswordHash.FromHashed(
                BcryptPasswordHasher.HashPassword(UserConstants.UserBob.RawPassword)));

        return user;
    }

    public static User CreateUserCarol()
    {
        var user = CreateUser(
            id: UserConstants.UserCarol.Id,
            email: UserConstants.UserCarol.Email,
            passwordHash: PasswordHash.FromHashed(
                BcryptPasswordHasher.HashPassword(UserConstants.UserCarol.RawPassword)));

        return user;
    }

    public static User CreateUserLexi()
    {
        var user = CreateUser(
            id: UserConstants.UserLexi.Id,
            email: UserConstants.UserLexi.Email,
            passwordHash: PasswordHash.FromHashed(
                BcryptPasswordHasher.HashPassword(UserConstants.UserLexi.RawPassword)));

        return user;
    }
}
