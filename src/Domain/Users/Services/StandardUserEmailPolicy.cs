using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class StandardUserEmailPolicy(IUserRepository userRepository)
    : IUserEmailPolicy
{
    public async Task<Result> ValidateEmailAsync(User user, EmailAddress newEmail)
    {
        return await userRepository.GetByEmailAsync(newEmail) is not null
            ? UserErrors.EmailAlreadyExists.ToResult()
            : Result.Success();
    }
}
