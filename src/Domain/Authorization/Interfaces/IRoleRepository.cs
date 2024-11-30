using AgendaManager.Domain.Authorization.ValueObjects;

namespace AgendaManager.Domain.Authorization.Interfaces;

public interface IRoleRepository
{
    IQueryable<Role> GetQueryAbleRoles();

    Task<Role?> GetByIdAsync(RoleId roleId, CancellationToken cancellationToken = default);

    Task<Role?> GetByIdWithPermissionsAsync(RoleId roleId, CancellationToken cancellationToken = default);

    Task<ICollection<Role>> GetAllWithPermissionsAsync(CancellationToken cancellationToken = default);

    Task<ICollection<Role>> GetByIdsWithPermissionsAsync(
        List<RoleId> roleIds,
        CancellationToken cancellationToken);

    Task<bool> ExistsByIdAsync(RoleId roleId, CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(Role role, CancellationToken cancellationToken = default);

    Task AddAsync(Role role, CancellationToken cancellationToken = default);

    void Update(Role role);
}
