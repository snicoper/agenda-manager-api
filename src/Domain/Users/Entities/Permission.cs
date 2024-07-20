using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class Permission : AuditableEntity
{
    private readonly HashSet<RolePermission> _rolePermissions = [];

    private Permission()
    {
    }

    private Permission(PermissionId id, string name)
    {
        Id = id;
        Name = name;
    }

    public PermissionId Id { get; set; } = null!;

    public string Name { get; set; } = default!;

    [NotMapped]
    public IReadOnlyCollection<Role> Roles => _rolePermissions
        .Select(up => up.Role)
        .ToList()
        .AsReadOnly();

    public static Permission Create(PermissionId id, string name)
    {
        return new Permission(id, name);
    }
}
