using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Role : AuditableEntity
{
    private readonly List<Permission> _permissions = [];
    private readonly List<User> _users = [];

    internal Role(RoleId roleId, string name, string description, bool editable = false)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = roleId;
        Name = name;
        Description = description;
        Editable = editable;

        AddDomainEvent(new RoleCreatedDomainEvent(Id));
    }

    private Role()
    {
    }

    public RoleId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool Editable { get; private set; }

    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();

    public void UpdateEditableState(bool editable)
    {
        if (Editable == editable)
        {
            return;
        }

        Editable = editable;

        AddDomainEvent(new RoleEditableStateUpdatedDomainEvent(Id, editable));
    }

    internal void UpdateRole(string name, string description)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        if (Name == name && Description == description)
        {
            return;
        }

        Name = name;
        Description = description;

        AddDomainEvent(new RoleUpdatedDomainEvent(Id));
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

    private static void GuardAgainstInvalidName(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > 100)
        {
            throw new RoleDomainException("Role name is null or exceeds length of 100 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        if (string.IsNullOrEmpty(description) || description.Length > 500)
        {
            throw new RoleDomainException("Role description is null or exceeds length of 100 characters.");
        }
    }
}
