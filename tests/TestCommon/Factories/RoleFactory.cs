using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRole(
        RoleId? roleId = null,
        string name = Roles.Admin,
        string description = "Admin role")
    {
        return new Role(roleId ?? RoleId.Create(), name, description);
    }
}
