using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class UserPasswordService(IPasswordPolicy passwordPolicy, IPasswordHasher passwordHasher)
{
    public Result<string> SetPassword(User user, string newPassword)
    {
        var validationResult = passwordPolicy.IsPasswordValid(newPassword);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<string>();
        }

        user.UpdatePassword(newPassword);

        return Result.Success<string>();
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return passwordHasher.VerifyPassword(password, hashedPassword);
    }
}
