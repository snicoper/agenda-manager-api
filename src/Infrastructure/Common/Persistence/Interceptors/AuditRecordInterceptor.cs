using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Infrastructure.Common.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AgendaManager.Infrastructure.Common.Persistence.Interceptors;

public class AuditRecordInterceptor(AuditRecordInterceptorService auditRecordInterceptorService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        RegisterAuditRecordsForAggregates(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        RegisterAuditRecordsForAggregates(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Register audit records for aggregates.
    /// </summary>
    private void RegisterAuditRecordsForAggregates(DbContext? context)
    {
        // Users.
        auditRecordInterceptorService.RecordAuditEntries<User>(
            context: context,
            entityId: nameof(User.Id),
            auditableProperties: [nameof(User.IsActive)]);

        // Roles.
        auditRecordInterceptorService.RecordAuditEntries<Role>(
            context: context,
            entityId: nameof(Role.Id),
            auditableProperties: [nameof(Role.Editable)]);
    }
}
