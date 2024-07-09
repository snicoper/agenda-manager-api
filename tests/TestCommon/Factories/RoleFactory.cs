using AgendaManager.Domain.Authorization;
using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRoleAdmin()
    {
        return Role.Create(Constants.Role.Id, Constants.Role.RoleAdmin);
    }

    public static Role CreateRoleManager()
    {
        return Role.Create(Constants.Role.Id, Constants.Role.RoleManager);
    }

    public static Role CreateRoleClient()
    {
        return Role.Create(Constants.Role.Id, Constants.Role.RoleClient);
    }
}
