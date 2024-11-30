using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Authorization.ValueObjects;

namespace AgendaManager.Domain.Authorization.Interfaces;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(PermissionId permissionId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(Permission permission, CancellationToken cancellationToken = default);

    Task AddAsync(Permission permission, CancellationToken cancellationToken = default);
}
