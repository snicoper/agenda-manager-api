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
            ? IdentityUserErrors.InvalidCredentials
            : Result.Success();
    }

    private static Result Validate(User user)
    {
        if (!user.Active)
        {
            return IdentityUserErrors.UserIsNotActive;
        }

        if (!user.IsEmailConfirmed)
        {
            return IdentityUserErrors.EmailIsNotConfirmed;
        }

        return Result.Success();
    }
}
