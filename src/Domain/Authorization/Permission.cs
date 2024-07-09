using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users;

namespace AgendaManager.Domain.Authorization;

public sealed class Permission : AuditableEntity
{
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

    public ICollection<UserPermission> UserPermissions { get; } = new HashSet<UserPermission>();

    [NotMapped]
    public IReadOnlyCollection<User> Users => UserPermissions.Select(up => up.User).ToList();

    public static Permission Create(PermissionId id, string name)
    {
        return new Permission(id, name);
    }
}
