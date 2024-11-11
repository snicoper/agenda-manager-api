using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class Permission : AuditableEntity
{
    internal Permission(PermissionId permissionId, string name)
    {
        ArgumentNullException.ThrowIfNull(name);

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

    private static void GuardAgainstInvalidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
        {
            throw new PermissionDomainException("Permission name is null or exceeds length of 100 characters.");
        }

        if (!name.EndsWith(":create") &&
            !name.EndsWith(":update") &&
            !name.EndsWith(":delete") &&
            !name.EndsWith(":read"))
        {
            throw new PermissionDomainException(
                "Permission name cannot end with ':create', ':update', ':delete', or ':read'.");
        }
    }
}
