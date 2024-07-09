using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users;

namespace AgendaManager.Domain.Authorization;

public sealed class Role : AuditableEntity
{
    private Role()
    {
    }

    private Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }

    public RoleId Id { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();

    [NotMapped]
    public IReadOnlyCollection<User> Users => UserRoles.Select(userRole => userRole.User).ToList();

    public static Role Create(RoleId id, string name)
    {
        return new Role(id, name);
    }
}
