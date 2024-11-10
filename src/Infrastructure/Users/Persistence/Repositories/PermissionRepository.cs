﻿using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users.Persistence.Repositories;

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

    public Task<bool> NameExistsAsync(Permission permission, CancellationToken cancellationToken = default)
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
