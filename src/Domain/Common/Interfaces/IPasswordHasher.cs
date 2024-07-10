using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Common.Interfaces;

public interface IPasswordHasher
{
    Result<string> HashPassword(string password);

    bool VerifyPassword(string password, string hashedPassword);
}
