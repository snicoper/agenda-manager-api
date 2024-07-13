using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class StrongPasswordPolicy : IPasswordPolicy
{
    public Result IsPasswordValid(string password)
    {
        return DomainRegex.StrongPassword().IsMatch(password) ? Result.Success() : UserErrors.InvalidFormatPassword;
    }
}
