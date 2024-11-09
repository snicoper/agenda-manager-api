using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class UserEmailManager(IUserRepository userRepository) : IUserEmailManager
{
    public async Task<Result> UpdateUserEmailAsync(User user, EmailAddress newEmail)
    {
        var validationResult = await ValidateEmailAsync(user, newEmail);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        user.UpdateEmail(newEmail);
        userRepository.Update(user);

        return Result.Success();
    }

    public async Task<Result> ValidateEmailAsync(User user, EmailAddress newEmail)
    {
        return await userRepository.GetByEmailAsync(newEmail) is not null
            ? UserErrors.EmailAlreadyExists
            : Result.Success();
    }
}
