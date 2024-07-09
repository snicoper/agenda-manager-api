using AgendaManager.Domain.Authorization;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class PermissionFactory
{
    public static Permission CreatePermissionUsersCanCreate()
    {
        return Permission.Create(Constants.Permissions.Id, Constants.Permissions.UsersCanCreate);
    }

    public static Permission CreatePermissionUsersCanUpdate()
    {
        return Permission.Create(Constants.Permissions.Id, Constants.Permissions.UsersCanUpdate);
    }

    public static Permission CreatePermissionUsersCanDelete()
    {
        return Permission.Create(Constants.Permissions.Id, Constants.Permissions.UsersCanDelete);
    }

    public static Permission CreatePermissionUsersCanRead()
    {
        return Permission.Create(Constants.Permissions.Id, Constants.Permissions.UsersCanRead);
    }
}
