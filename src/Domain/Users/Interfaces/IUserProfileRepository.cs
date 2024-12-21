using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserProfileRepository
{
    Task<bool> ExistsIdentityDocumentAsync(
        UserId userId,
        IdentityDocument identityDocument,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsPhoneNumberAsync(
        UserId userId,
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);
}
