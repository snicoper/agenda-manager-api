using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

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
}
