using AgendaManager.Domain.Common.ValueObjects.EmailAddress;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IEmailUniquenessPolicy
{
    Task<bool> IsUnique(EmailAddress email, CancellationToken cancellationToken = default);
}
