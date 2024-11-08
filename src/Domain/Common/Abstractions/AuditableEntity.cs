using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Common.Abstractions;

public abstract class AuditableEntity : Entity, IAuditableEntity
{
    public string CreatedBy { get; set; } = default!;

    public DateTimeOffset CreatedAt { get; set; }

    public string LastModifiedBy { get; set; } = default!;

    public DateTimeOffset LastModifiedAt { get; set; }

    public int Version { get; set; }
}
