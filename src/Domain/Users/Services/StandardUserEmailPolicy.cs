using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class StandardUserEmailPolicy(IUserRepository userRepository)
    : IUserEmailPolicy
{
    public async Task<Result> ValidateEmailAsync(User user, EmailAddress newEmail)
    {
        return await userRepository.GetByEmailAsync(newEmail) is not null
            ? UserErrors.EmailAlreadyExists
            : Result.Success();
    }
}
