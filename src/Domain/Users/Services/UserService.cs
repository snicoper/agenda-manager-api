using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class UserService(IUserRepository userRepository)
{
    public async Task<Result<User>> CreateAsync(
        UserId userId,
        EmailAddress email,
        string passwordHash,
        string? firstName,
        string? lastName,
        bool active = true,
        bool confirmEmail = false,
        CancellationToken cancellationToken = default)
    {
        var user = User.Create(userId, email, passwordHash, firstName, lastName);

        var createdValidationResult = await ValidateAsync(user, cancellationToken);
        if (createdValidationResult.IsFailure)
        {
            return createdValidationResult.MapToValue<User>();
        }

        var newUser = User.Create(user.Id, user.Email, user.PasswordHash, user.FirstName, user.LastName);
        newUser.SetActiveState(active);

        if (confirmEmail)
        {
            newUser.ConfirmEmail();
        }

        await userRepository.AddAsync(newUser, cancellationToken);

        return Result.Create(newUser);
    }

    public async Task<Result> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var updatedValidationResult = await ValidateAsync(user, cancellationToken);
        if (updatedValidationResult.IsFailure)
        {
            return updatedValidationResult.MapToValue<User>();
        }

        user.UpdateUser(user.FirstName, user.LastName);
        userRepository.Update(user);

        return Result.Success();
    }

    public async Task<Result> ValidateAsync(User user, CancellationToken cancellationToken)
    {
        // FirstName and LastName cannot be longer than 256 characters.
        if (!string.IsNullOrWhiteSpace(user.FirstName) && user.FirstName.Length > 256)
        {
            return UserErrors.FirstNameExceedsLength;
        }

        if (!string.IsNullOrWhiteSpace(user.LastName) && user.LastName.Length > 256)
        {
            return UserErrors.LastNameExceedsLength;
        }

        if (await EmailExistsAsync(user, cancellationToken))
        {
            return UserErrors.EmailAlreadyExists;
        }

        return Result.Success();
    }

    public async Task<bool> EmailExistsAsync(User user, CancellationToken cancellationToken)
    {
        var emailIsUnique = await userRepository.EmailExistsAsync(user, cancellationToken);

        return emailIsUnique;
    }
}
