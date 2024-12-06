﻿using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class UserManager(IUserRepository userRepository)
{
    public async Task<Result<User>> CreateUserAsync(
        UserId userId,
        EmailAddress email,
        PasswordHash passwordHash,
        string firstName,
        string lastName,
        bool active = true,
        bool isAssignableResource = false,
        bool emailConfirmed = false,
        CancellationToken cancellationToken = default)
    {
        User user = new(
            userId: userId,
            email: email,
            passwordHash: passwordHash,
            isActive: active,
            isAssignableResource: isAssignableResource,
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

    private static void CreateUserProfile(User user, string firstName, string lastName)
    {
        var userProfile = UserProfile.Create(UserProfileId.Create(), user.Id, firstName, lastName);
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
        var emailIsUnique = await userRepository.EmailExistsAsync(email, cancellationToken);

        return emailIsUnique;
    }
}
