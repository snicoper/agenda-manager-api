using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(PermissionId permissionId, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(Permission permission, CancellationToken cancellationToken = default);

    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
}
