using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken = default);

    Task<Role?> GetByIdWithPermissionsAsync(RoleId roleId, CancellationToken cancellationToken = default);
}
