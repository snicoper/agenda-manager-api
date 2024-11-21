using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class UserRoleFactory
{
    public static UserRole CreateUserRole(UserId? userId = null, RoleId? roleId = null)
    {
        return UserRole.Create(userId ?? UserId.Create(), roleId ?? RoleId.Create());
    }
}
