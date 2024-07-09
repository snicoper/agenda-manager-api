using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users;

namespace AgendaManager.Domain.Authorization;

public class Role : AuditableEntity
{
    public Role()
    {
    }

    public Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }

    public RoleId Id { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();

    [NotMapped]
    public IReadOnlyCollection<User> Users => UserRoles.Select(userRole => userRole.User).ToList();
}
