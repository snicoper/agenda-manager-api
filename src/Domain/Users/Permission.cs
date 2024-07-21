using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Permission : AuditableEntity
{
    private readonly List<Role> _roles = [];

    private Permission()
    {
    }

    private Permission(PermissionId id, string name)
    {
        Id = id;
        Name = name;
    }

    public PermissionId Id { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static Permission Create(PermissionId id, string name)
    {
        return new Permission(id, name);
    }
}
