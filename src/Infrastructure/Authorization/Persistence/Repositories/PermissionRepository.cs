using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Authorization.Persistence.Repositories;

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

    public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await context.Permissions.ToListAsync(cancellationToken);

        return permissions;
    }

    public Task<bool> ExistsByNameAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        var nameIsUnique = context
            .Permissions
            .AnyAsync(r => r.Name == permission.Name && r.Id != permission.Id, cancellationToken);

        return nameIsUnique;
    }

    public async Task AddAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        await context.Permissions.AddAsync(permission, cancellationToken);
    }
}
