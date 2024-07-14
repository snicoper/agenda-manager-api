namespace AgendaManager.Domain.Users.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string rawPassword);

    bool VerifyPassword(string rawPassword, string hashedPassword);
}
