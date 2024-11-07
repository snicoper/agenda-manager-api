using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRoleAdmin(RoleId? roleId = null, string name = Roles.Admin)
    {
        return new Role(roleId ?? RoleId.Create(), name);
    }

    public static Role CreateRoleManager(RoleId? roleId = null, string name = Roles.Manager)
    {
        return new Role(roleId ?? RoleId.Create(), name);
    }

    public static Role CreateRoleClient(RoleId? roleId = null, string name = Roles.Client)
    {
        return new Role(roleId ?? RoleId.Create(), name);
    }
}
