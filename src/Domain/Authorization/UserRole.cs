using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization;

public class UserRole : AuditableEntity
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

    public User User { get; private set; } = null!;

    public RoleId RoleId { get; private set; } = null!;

    public Role Role { get; private set; } = null!;
}
