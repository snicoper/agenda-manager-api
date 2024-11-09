using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string rawPassword);

    bool VerifyPassword(string rawPassword, PasswordHash passwordHash);
}
