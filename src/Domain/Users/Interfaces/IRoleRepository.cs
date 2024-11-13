﻿using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(RoleId roleId, CancellationToken cancellationToken);

    Task<Role?> GetByIdWithPermissionsAsync(RoleId roleId, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(Role role, CancellationToken cancellationToken = default);

    Task AddAsync(Role role, CancellationToken cancellationToken = default);

    void Update(Role role);
}
