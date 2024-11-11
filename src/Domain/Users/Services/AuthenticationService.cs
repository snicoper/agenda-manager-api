using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class AuthenticationService(IPasswordHasher passwordHasher)
{
    public Result AuthenticateUser(User user, string rawPassword)
    {
        var validationResult = Validate(user);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        return user.PasswordHash.Verify(rawPassword, passwordHasher) is false
            ? UserErrors.InvalidCredentials
            : Result.Success();
    }

    private static Result Validate(User user)
    {
        if (!user.IsActive)
        {
            return UserErrors.UserIsNotActive;
        }

        return !user.IsEmailConfirmed ? UserErrors.EmailIsNotConfirmed : Result.Success();
    }
}
