namespace AgendaManager.Domain.Common.Interfaces;

public interface IAuditableEntity
{
    DateTimeOffset Created { get; set; }

    Guid CreatedBy { get; set; }

    DateTimeOffset LastModified { get; set; }

    Guid LastModifiedBy { get; set; }
}
