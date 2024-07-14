using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Services;

public class UserAuthenticationService(UserPasswordService userPasswordService)
{
    public Result AuthenticateUser(User user, string password)
    {
        var validationResult = ValidateUser(user);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        return !userPasswordService.VerifyPassword(password, user.PasswordHash)
            ? UserErrors.InvalidCredentials
            : Result.Success();
    }

    private static Result ValidateUser(User user)
    {
        if (!user.Active)
        {
            return UserErrors.UserIsNotActive;
        }

        if (!user.IsEmailConfirmed)
        {
            return UserErrors.EmailIsNotConfirmed;
        }

        return Result.Success();
    }
}
