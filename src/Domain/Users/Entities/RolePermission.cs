using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class RolePermission : AuditableEntity
{
    private RolePermission(RoleId roleId, PermissionId permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    private RolePermission()
    {
    }

    public RoleId RoleId { get; private set; } = null!;

    public Role Role { get; private set; } = null!;

    public PermissionId PermissionId { get; private set; } = null!;

    public Permission Permission { get; private set; } = null!;

    public static RolePermission Create(RoleId roleId, PermissionId permissionId)
    {
        return new RolePermission(roleId, permissionId);
    }
}
