using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRoleAdmin()
    {
        return Role.Create(RoleId.Create(), Roles.Admin);
    }

    public static Role CreateRoleManager()
    {
        return Role.Create(RoleId.Create(), Roles.Manager);
    }

    public static Role CreateRoleClient()
    {
        return Role.Create(RoleId.Create(), Roles.Client);
    }
}
