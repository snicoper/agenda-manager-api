using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class UserPasswordService(IPasswordPolicy passwordPolicy, IPasswordHasher passwordHasher)
{
    public Result<string> CreatePassword(string newPassword)
    {
        var validationResult = passwordPolicy.IsPasswordValid(newPassword);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<string>();
        }

        var hashedPassword = passwordHasher.HashPassword(newPassword);

        return Result.Success(hashedPassword);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return passwordHasher.VerifyPassword(password, hashedPassword);
    }
}
