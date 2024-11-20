using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;

namespace AgendaManager.Infrastructure.AuditRecords.Persistence.Repositories;

public class AuditRecordRepository(AppDbContext context) : IAuditRecordRepository
{
    public async Task AddAsync(AuditRecord auditRecord, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(auditRecord, cancellationToken);
    }
}
