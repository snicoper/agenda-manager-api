using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization;

public class UserRole
{
    private UserRole(UserId userId, RoleId roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public UserId UserId { get; private set; }

    public User User { get; private set; } = default!;

    public RoleId RoleId { get; private set; }

    public Role Role { get; private set; } = default!;
}
