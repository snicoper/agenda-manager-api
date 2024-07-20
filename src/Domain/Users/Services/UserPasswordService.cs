using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class UserPasswordService(IPasswordPolicy passwordPolicy, IPasswordHasher passwordHasher)
{
    public Result SetPassword(User user, string rawPassword)
    {
        var validationResult = passwordPolicy.IsPasswordValid(rawPassword);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        var hashedPassword = passwordHasher.HashPassword(rawPassword);
        user.UpdatePassword(hashedPassword);

        return Result.Success();
    }

    public bool VerifyPassword(string rawPassword, string hashedPassword)
    {
        return passwordHasher.VerifyPassword(rawPassword, hashedPassword);
    }
}
