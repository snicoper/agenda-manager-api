using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Persistence;

public interface IPasswordManager
{
    public Result<string> HashPassword(string password);

    bool IsValidPassword(string password, string hash);
}
