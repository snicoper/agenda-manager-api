using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Authorization.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class PermissionFactory
{
    public static Permission CreatePermission(
        PermissionId? permissionId = null,
        string? name = null)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name ?? SystemPermissions.Users.Create);
    }

    public static Permission CreatePermissionUsersCreate(
        PermissionId? permissionId = null,
        string name = SystemPermissions.Users.Create)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersUpdate(
        PermissionId? permissionId = null,
        string name = SystemPermissions.Users.Update)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersDelete(
        PermissionId? permissionId = null,
        string name = SystemPermissions.Users.Delete)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersRead(
        PermissionId? permissionId = null,
        string name = SystemPermissions.Users.Read)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }
}
