using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.Policies;

public class EmailUniquenessPolicy(IUserRepository userRepository) : IEmailUniquenessPolicy
{
    public async Task<bool> IsUnique(EmailAddress email, CancellationToken cancellationToken = default)
    {
        var emailExists = await userRepository.EmailExistsAsync(email, cancellationToken);

        return !emailExists;
    }
}
