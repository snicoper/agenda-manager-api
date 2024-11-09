using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class PasswordPolicy : IPasswordPolicy
{
    public Result ValidatePassword(string rawPassword)
    {
        var validateResult = DomainRegex.StrongPassword().IsMatch(rawPassword)
            ? Result.Success()
            : UserErrors.InvalidFormatPassword;

        return validateResult;
    }
}
