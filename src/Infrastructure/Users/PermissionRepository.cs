using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users;

public class PermissionRepository(AppDbContext context) : IPermissionRepository
{
    public async Task<Permission?> GetByIdAsync(
        PermissionId permissionId,
        CancellationToken cancellationToken = default)
    {
        var permission = await context
            .Permissions
            .FirstOrDefaultAsync(r => r.Id.Equals(permissionId), cancellationToken);

        return permission;
    }
}
