using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class UserUserPasswordManager(IPasswordHasher passwordHasher) : IUserPasswordManager
{
    public Result ChangePassword(User user, string rawPassword)
    {
        var isPasswordValid = IsPasswordValid(rawPassword);

        if (isPasswordValid.IsFailure)
        {
            return isPasswordValid;
        }

        var hashedPassword = passwordHasher.HashPassword(rawPassword);

        user.UpdatePassword(hashedPassword);

        return Result.Success();
    }

    public Result IsPasswordValid(string rawPassword)
    {
        return DomainRegex.StrongPassword().IsMatch(rawPassword)
            ? Result.Success()
            : UserErrors.InvalidFormatPassword;
    }

    public bool VerifyPassword(string rawPassword, PasswordHash passwordHash)
    {
        return passwordHasher.VerifyPassword(rawPassword, passwordHash);
    }
}
