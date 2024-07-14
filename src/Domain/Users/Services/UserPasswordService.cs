using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class UserPasswordService(IPasswordPolicy passwordPolicy, IPasswordHasher passwordHasher)
{
    public Result<string> SetPassword(User user, string rawPassword)
    {
        var validationResult = passwordPolicy.IsPasswordValid(rawPassword);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<string>();
        }

        var hashedPassword = passwordHasher.HashPassword(rawPassword);
        user.UpdatePassword(hashedPassword);

        return Result.Success<string>();
    }

    public bool VerifyPassword(string rawPassword, string hashedPassword)
    {
        return passwordHasher.VerifyPassword(rawPassword, hashedPassword);
    }
}
