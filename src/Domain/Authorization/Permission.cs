using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users;

namespace AgendaManager.Domain.Authorization;

public class Permission : AuditableEntity
{
    public Permission()
    {
    }

    public Permission(PermissionId id, string name)
    {
        Id = id;
        Name = name;
    }

    public PermissionId Id { get; set; } = null!;

    public string Name { get; set; } = default!;

    public virtual ICollection<UserPermission> UserPermissions { get; } = new HashSet<UserPermission>();

    [NotMapped]
    public IEnumerable<User> Users => UserPermissions.Select(up => up.User).ToList();
}
