using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class UserEmailService(IUserRepository userRepository, IUserEmailPolicy userEmailPolicy)
{
    public async Task<Result> UpdateUserEmailAsync(User user, EmailAddress newEmail)
    {
        var validationResult = await userEmailPolicy.ValidateEmailAsync(user, newEmail);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        user.UpdateEmail(newEmail);
        userRepository.Update(user);

        return Result.Success();
    }
}
