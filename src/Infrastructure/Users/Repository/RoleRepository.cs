using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users.Repository;

public class RoleRepository(AppDbContext context)
    : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken = default)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId), cancellationToken);

        return role;
    }

    public async Task<Role?> GetByIdWithPermissionsAsync(RoleId roleId, CancellationToken cancellationToken = default)
    {
        var role = await context
            .Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id.Equals(roleId), cancellationToken);

        return role;
    }
}
