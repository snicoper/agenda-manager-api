using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization;

public class UserPermission : AuditableEntity
{
    private UserPermission()
    {
    }

    private UserPermission(UserId userId, PermissionId permissionId)
    {
        UserId = userId;
        PermissionId = permissionId;
    }

    public UserId UserId { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public PermissionId PermissionId { get; private set; } = null!;

    public Permission Permission { get; private set; } = null!;
}
