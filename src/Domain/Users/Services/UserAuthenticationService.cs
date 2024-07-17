using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Services;

public class UserAuthenticationService(UserPasswordService userPasswordService)
{
    public Result AuthenticateUser(User user, string password)
    {
        var validationResult = Validate(user);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        return userPasswordService.VerifyPassword(password, user.PasswordHash) is false
            ? UserErrors.InvalidCredentials
            : Result.Success();
    }

    private static Result Validate(User user)
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
