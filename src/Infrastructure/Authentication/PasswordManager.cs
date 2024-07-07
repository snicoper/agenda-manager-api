using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Users.Persistence;

namespace AgendaManager.Infrastructure.Authentication;

public class PasswordManager : IPasswordManager
{
    public Result<string> HashPassword(string password)
    {
        return !DomainRegex.StrongPassword().IsMatch(password)
            ? Error.Validation("Password", "Password is not strong enough").ToResult<string>()
            : Result.Success(BCrypt.Net.BCrypt.EnhancedHashPassword(password));
    }

    public bool IsValidPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
