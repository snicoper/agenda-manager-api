using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Common.Utils;

public static class PasswordHasher
{
    public static Result<string> HashPassword(string password)
    {
        return !DomainRegex.StrongPassword().IsMatch(password)
            ? Error.Validation("Password", "Password is not strong enough").ToResult<string>()
            : Result.Success(BCrypt.Net.BCrypt.EnhancedHashPassword(password));
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
