using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class PermissionFactory
{
    public static Permission CreatePermission(
        PermissionId? permissionId = null,
        string? name = null)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name ?? PermissionNames.Users.Create);
    }

    public static Permission CreatePermissionUsersCreate(
        PermissionId? permissionId = null,
        string name = PermissionNames.Users.Create)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersUpdate(
        PermissionId? permissionId = null,
        string name = PermissionNames.Users.Update)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersDelete(
        PermissionId? permissionId = null,
        string name = PermissionNames.Users.Delete)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersRead(
        PermissionId? permissionId = null,
        string name = PermissionNames.Users.Read)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }
}
