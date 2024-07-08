using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

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
}
