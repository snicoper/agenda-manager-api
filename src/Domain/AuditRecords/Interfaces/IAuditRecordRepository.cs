namespace AgendaManager.Domain.AuditRecords.Interfaces;

public interface IAuditRecordRepository
{
    Task AddAsync(AuditRecord auditRecord, CancellationToken cancellationToken = default);
}
