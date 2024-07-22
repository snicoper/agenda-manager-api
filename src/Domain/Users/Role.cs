using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Role : AggregateRoot
{
    private readonly List<Permission> _permissions = [];
    private readonly List<User> _users = [];

    private Role()
    {
    }

    private Role(RoleId id, string name, bool editable)
    {
        Id = id;
        Name = name;
        Editable = editable;
    }

    public RoleId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public bool Editable { get; private set; }

    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public static Role Create(RoleId id, string name, bool editable = false)
    {
        return new Role(id, name, editable);
    }

    internal Result AddPermission(Permission permission)
    {
        _permissions.Add(permission);

        AddDomainEvent(new RolePermissionAddedDomainEvent(Id, permission.Id));

        return Result.Success();
    }

    internal Result RemovePermission(Permission permission)
    {
        _permissions.Remove(permission);

        AddDomainEvent(new RolePermissionRemovedDomainEvent(Id, permission.Id));

        return Result.Success();
    }
}
