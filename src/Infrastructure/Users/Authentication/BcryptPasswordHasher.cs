using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Infrastructure.Users.Authentication;

public class BcryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string rawPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(rawPassword);
    }

    public bool VerifyPassword(string rawPassword, PasswordHash passwordHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(rawPassword, passwordHash.HashedValue);
    }
}
