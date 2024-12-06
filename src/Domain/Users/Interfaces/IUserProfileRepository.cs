using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserProfileRepository
{
    Task<bool> IdentityDocumentExistsAsync(
        IdentityDocument identityDocument,
        CancellationToken cancellationToken = default);
}
