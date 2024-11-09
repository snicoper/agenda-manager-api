using AgendaManager.Domain.Common.ValueObjects.EmailAddress;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUnique(EmailAddress email, CancellationToken cancellationToken = default);
}
