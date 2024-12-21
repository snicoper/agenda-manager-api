using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users.Repositories;

public class UserProfileRepository(AppDbContext context) : IUserProfileRepository
{
    public async Task<bool> ExistsIdentityDocumentAsync(
        UserId userId,
        IdentityDocument identityDocument,
        CancellationToken cancellationToken = default)
    {
        var identityDocumentExists = await context.UserProfiles
            .AsNoTracking()
            .AnyAsync(
                up =>
                    up.IdentityDocument != null &&
                    up.IdentityDocument.Value == identityDocument.Value &&
                    up.IdentityDocument.CountryCode == identityDocument.CountryCode &&
                    up.IdentityDocument.Type == identityDocument.Type &&
                    up.UserId != userId,
                cancellationToken);

        return identityDocumentExists;
    }

    public Task<bool> ExistsPhoneNumberAsync(
        UserId userId,
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        var phoneNumberExists = context.UserProfiles
            .AsNoTracking()
            .AnyAsync(
                up =>
                    up.PhoneNumber != null &&
                    up.PhoneNumber.Number == phoneNumber.Number &&
                    up.PhoneNumber.CountryCode == phoneNumber.CountryCode &&
                    up.UserId != userId,
                cancellationToken);

        return phoneNumberExists;
    }
}
