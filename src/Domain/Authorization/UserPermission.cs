using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization;

public class UserPermission
{
    private UserPermission(UserId userId, PermissionId permissionId)
    {
        UserId = userId;
        PermissionId = permissionId;
    }

    public UserId UserId { get; private set; }

    public User User { get; private set; } = default!;

    public PermissionId PermissionId { get; private set; }

    public Permission Permission { get; private set; } = default!;
}
