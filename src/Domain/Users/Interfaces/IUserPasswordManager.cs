using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserPasswordManager
{
    Result ChangePassword(User user, string rawPassword);

    Result IsPasswordValid(string rawPassword);

    bool VerifyPassword(string rawPassword, PasswordHash passwordHash);
}
