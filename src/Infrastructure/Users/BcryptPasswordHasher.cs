using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Infrastructure.Users;

public class BcryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string rawPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(rawPassword);
    }

    public bool VerifyPassword(string rawPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(rawPassword, hashedPassword);
    }
}
