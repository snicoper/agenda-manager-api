using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Authorization.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RoleFactory
{
    public static Role CreateRole(
        RoleId? roleId = null,
        string? name = null,
        string? description = null,
        bool? isEditable = null)
    {
        return new Role(
            roleId ?? RoleId.Create(),
            name ?? SystemRoles.Administrator,
            description ?? "Role description",
            isEditable ?? false);
    }
}
