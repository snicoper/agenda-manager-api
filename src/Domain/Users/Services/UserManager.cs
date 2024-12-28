using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public sealed class UserManager(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy)
{
    public async Task<Result<User>> CreateUserAsync(
        UserId userId,
        EmailAddress email,
        string passwordRaw,
        string firstName,
        string lastName,
        bool active = true,
        bool emailConfirmed = false,
        CancellationToken cancellationToken = default)
    {
        var passwordHash = PasswordHash.FromRaw(passwordRaw, passwordHasher, passwordPolicy);

        if (passwordHash.IsFailure)
        {
            return passwordHash.MapTo<User>();
        }

        User user = new(
            userId: userId,
            email: email,
            passwordHash: passwordHash.Value!,
            isActive: active,
            emailConfirmed: emailConfirmed);

        var validationResult = await IsValidAsync(user, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<User>();
        }

        // Create user profile.
        CreateUserProfile(user, firstName, lastName);

        await userRepository.AddAsync(user, cancellationToken);

        return Result.Create(user);
    }

    public Result UpdatePassword(User user, string newPasswordRaw)
    {
        var passwordHash = PasswordHash.FromRaw(newPasswordRaw, passwordHasher, passwordPolicy);

        if (passwordHash.IsFailure)
        {
            return passwordHash;
        }

        var updatePasswordResult = user.UpdatePassword(passwordHash.Value!);

        return updatePasswordResult;
    }

    private static void CreateUserProfile(User user, string firstName, string lastName)
    {
        var userProfile = UserProfile.Create(
            userProfileId: UserProfileId.Create(),
            userId: user.Id,
            firstName: firstName,
            lastName: lastName,
            phoneNumber: null,
            address: null,
            identityDocument: null);

        user.AddProfile(userProfile);
    }

    private async Task<Result> IsValidAsync(User user, CancellationToken cancellationToken)
    {
        if (await EmailExistsAsync(user.Email, cancellationToken))
        {
            return UserErrors.EmailAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> EmailExistsAsync(EmailAddress email, CancellationToken cancellationToken)
    {
        var emailIsUnique = await userRepository.ExistsEmailAsync(email, cancellationToken);

        return emailIsUnique;
    }
}
