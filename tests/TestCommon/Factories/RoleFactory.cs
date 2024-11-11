using AgendaManager.Domain.Users.Constants;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRole(
        RoleId? roleId = null,
        string name = SystemRoles.Administrator,
        string description = "Admin role")
    {
        return new Role(roleId ?? RoleId.Create(), name, description);
    }
}
