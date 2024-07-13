using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Services;

public class UserAuthenticationService(UserPasswordService userPasswordService)
{
    public Result AuthenticateUser(User user, string password)
    {
        if (!user.Active)
        {
            return UserErrors.UserIsNotActive.ToResult();
        }

        if (!user.IsEmailConfirmed)
        {
            return UserErrors.EmailIsNotConfirmed.ToResult();
        }

        return !userPasswordService.VerifyPassword(password, user.PasswordHash)
            ? UserErrors.InvalidCredentials
            : Result.Success();
    }
}
