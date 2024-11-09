using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Services;

public class EmailUniquenessChecker(IUserRepository userRepository) : IEmailUniquenessChecker
{
    public async Task<bool> IsUnique(EmailAddress email, CancellationToken cancellationToken = default)
    {
        var emailExists = await userRepository.EmailExistsAsync(email, cancellationToken);

        return !emailExists;
    }
}
