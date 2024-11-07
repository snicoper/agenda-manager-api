using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Permission : AuditableEntity
{
    private readonly List<Role> _roles = [];

    internal Permission(PermissionId permissionId, string name)
    {
        Id = permissionId;
        Name = name;

        AddDomainEvent(new PermissionCreatedDomainEvent(permissionId));
    }

    private Permission()
    {
    }

    public PermissionId Id { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
}
