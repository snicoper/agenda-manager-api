using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
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
        string? firstName,
        string? lastName,
        bool active = true,
        bool emailConfirmed = false,
        CancellationToken cancellationToken = default)
    {
        var user = User.Create(userId, email, passwordHash, firstName, lastName);

        user.UpdateActiveState(active);

        if (emailConfirmed)
        {
            user.SetEmailConfirmed();
        }

        var validationResult = await IsValidAsync(user, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<User>();
        }

        await userRepository.AddAsync(user, cancellationToken);

        return Result.Create(user);
    }

    public async Task<Result> UpdateUserAsync(
        User user,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        user.UpdateUser(firstName, lastName);
        var validationResult = await IsValidAsync(user, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        user.UpdateUser(user.FirstName, user.LastName);
        userRepository.Update(user);

        return Result.Success();
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
