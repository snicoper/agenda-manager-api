using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class PermissionFactory
{
    public static Permission CreatePermissionUsersCreate()
    {
        return Permission.Create(PermissionId.Create(), Permissions.User.Create);
    }

    public static Permission CreatePermissionUsersUpdate()
    {
        return Permission.Create(PermissionId.Create(), Permissions.User.Update);
    }

    public static Permission CreatePermissionUsersDelete()
    {
        return Permission.Create(PermissionId.Create(), Permissions.User.Delete);
    }

    public static Permission CreatePermissionUsersRead()
    {
        return Permission.Create(PermissionId.Create(), Permissions.User.Read);
    }
}
