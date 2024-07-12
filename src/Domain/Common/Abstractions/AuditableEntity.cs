using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Common.Abstractions;

public abstract class AuditableEntity : Entity, IAuditableEntity
{
    public Guid CreatedBy { get; set; }

    public DateTimeOffset Created { get; set; }

    public Guid LastModifiedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }
}
