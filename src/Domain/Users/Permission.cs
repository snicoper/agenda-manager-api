﻿using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Permission : AuditableEntity
{
    private readonly List<Role> _roles = [];

    internal Permission(PermissionId permissionId, string name)
    {
        GuardAgainstInvalidName(name);

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

    private static void GuardAgainstInvalidName(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > 100)
        {
            throw new PermissionDomainException("Permission name is null or exceeds length of 100 characters.");
        }
    }
}
