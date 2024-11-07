using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class PermissionFactory
{
    public static Permission CreatePermissionUsersCreate(
        PermissionId? permissionId = null,
        string name = Permissions.User.Create)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersUpdate(
        PermissionId? permissionId = null,
        string name = Permissions.User.Update)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersDelete(
        PermissionId? permissionId = null,
        string name = Permissions.User.Delete)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }

    public static Permission CreatePermissionUsersRead(
        PermissionId? permissionId = null,
        string name = Permissions.User.Read)
    {
        return new Permission(permissionId ?? PermissionId.Create(), name);
    }
}
