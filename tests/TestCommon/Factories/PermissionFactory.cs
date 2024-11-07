using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class PermissionFactory
{
    public static Permission CreatePermissionUsersCreate()
    {
        return new Permission(PermissionId.Create(), Permissions.User.Create);
    }

    public static Permission CreatePermissionUsersUpdate()
    {
        return new Permission(PermissionId.Create(), Permissions.User.Update);
    }

    public static Permission CreatePermissionUsersDelete()
    {
        return new Permission(PermissionId.Create(), Permissions.User.Delete);
    }

    public static Permission CreatePermissionUsersRead()
    {
        return new Permission(PermissionId.Create(), Permissions.User.Read);
    }
}
