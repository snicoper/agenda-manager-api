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

    public Task<bool> ExistsByIdAsync(RoleId roleId, CancellationToken cancellationToken)
    {
        var exists = context.Roles.AnyAsync(r => r.Id.Equals(roleId), cancellationToken);

        return exists;
    }

    public async Task<Role?> GetByIdWithPermissionsAsync(RoleId roleId, CancellationToken cancellationToken = default)
    {
        var role = await context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id.Equals(roleId), cancellationToken);

        return role;
    }

    public async Task<bool> NameExistsAsync(Role role, CancellationToken cancellationToken = default)
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
