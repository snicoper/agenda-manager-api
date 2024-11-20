using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.Constants;

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
