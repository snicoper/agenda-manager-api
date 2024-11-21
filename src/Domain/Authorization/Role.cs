using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Events;
using AgendaManager.Domain.Authorization.Exceptions;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization;

public sealed class Role : AggregateRoot
{
    private readonly List<Permission> _permissions = [];

    internal Role(RoleId roleId, string name, string description, bool isEditable = false)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = roleId;
        Name = name;
        Description = description;
        IsEditable = isEditable;

        AddDomainEvent(new RoleCreatedDomainEvent(Id));
    }

    private Role()
    {
    }

    public RoleId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool IsEditable { get; private set; }

    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

    internal void Update(string name, string description)
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
        if (HasPermission(permission.Id))
        {
            return RoleErrors.PermissionAlreadyExistsInRole;
        }

        _permissions.Add(permission);

        AddDomainEvent(new RolePermissionAddedDomainEvent(Id, permission.Id));

        return Result.Success();
    }

    internal Result RemovePermission(Permission permission)
    {
        if (!HasPermission(permission.Id))
        {
            return RoleErrors.PermissionNotFoundInRole;
        }

        _permissions.Remove(permission);

        AddDomainEvent(new RolePermissionRemovedDomainEvent(Id, permission.Id));

        return Result.Success();
    }

    internal bool HasPermission(PermissionId permissionId)
    {
        return _permissions.Any(p => p.Id == permissionId);
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
        {
            throw new RoleDomainException("Role name is null or exceeds length of 100 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new RoleDomainException("Role description is null or exceeds length of 100 characters.");
        }
    }
}
