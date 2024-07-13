using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IPasswordPolicy
{
    Result IsPasswordValid(string password);
}
