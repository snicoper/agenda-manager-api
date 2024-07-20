using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class Role : AuditableEntity
{
    private readonly HashSet<RolePermission> _rolePermissions = [];
    private readonly HashSet<UserRole> _userRoles = [];

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

    [NotMapped]
    public IReadOnlyCollection<Permission> Permissions => _rolePermissions
        .Select(userRole => userRole.Permission)
        .ToList()
        .AsReadOnly();

    [NotMapped]
    public IReadOnlyCollection<User> Users => _userRoles
        .Select(userRole => userRole.User)
        .ToList()
        .AsReadOnly();

    public static Role Create(RoleId id, string name)
    {
        return new Role(id, name);
    }
}
