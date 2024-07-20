using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Role : AuditableEntity
{
    private readonly List<Permission> _permissions = [];
    private readonly List<User> _users = [];

    private Role()
    {
    }

    private Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }

    public RoleId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public static Role Create(RoleId id, string name)
    {
        return new Role(id, name);
    }

    public Result AddPermission(Permission permission)
    {
        if (_permissions.Any(p => p.Id.Equals(permission.Id)))
        {
            return IdentityUserErrors.RoleAlreadyExists;
        }

        _permissions.Add(permission);

        AddDomainEvent(new RolePermissionAddedDomainEvent(Id, permission.Id));

        return Result.Success();
    }
}
