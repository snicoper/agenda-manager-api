using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class UserRole : AuditableEntity
{
    private UserRole()
    {
    }

    private UserRole(UserId userId, RoleId roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public UserId UserId { get; private set; } = null!;

    public RoleId RoleId { get; private set; } = null!;

    public static UserRole Create(UserId userId, RoleId roleId)
    {
        return new UserRole(userId, roleId);
    }
}
