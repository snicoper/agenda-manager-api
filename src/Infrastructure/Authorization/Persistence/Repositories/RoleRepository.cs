using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Authorization.Persistence.Repositories;

public class RoleRepository(AppDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken = default)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId), cancellationToken);

        return role;
    }

    public async Task<List<Role>> GetByIdsWithPermissionsAsync(
        List<RoleId> roleIds,
        CancellationToken cancellationToken)
    {
        var roles = await context.Roles
            .Include(r => r.Permissions)
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        return roles;
    }

    public Task<bool> ExistsByIdAsync(RoleId roleId, CancellationToken cancellationToken)
    {
        var exists = context.Roles.AnyAsync(r => r.Id.Equals(roleId), cancellationToken);

        return exists;
    }

    public async Task<bool> ExistsByNameAsync(Role role, CancellationToken cancellationToken = default)
    {
        var nameIsUnique = await context
            .Roles
            .AnyAsync(r => r.Name == role.Name && r.Id != role.Id, cancellationToken);

        return nameIsUnique;
    }

    public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        await context.Roles.AddAsync(role, cancellationToken);
    }

    public void Update(Role role)
    {
        context.Update(role);
    }
}
