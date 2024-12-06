using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users.Repositories;

public class UserProfileRepository(AppDbContext context) : IUserProfileRepository
{
    public async Task<bool> IdentityDocumentExistsAsync(
        IdentityDocument identityDocument,
        CancellationToken cancellationToken = default)
    {
        var identityDocumentExists = await context.UserProfiles
            .AnyAsync(up => up.IdentityDocument == identityDocument, cancellationToken);

        return identityDocumentExists;
    }
}
