using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Users.Persistence.Interceptors;

public class UserAuditInterceptor(
    AuditRecordInterceptorService auditRecordInterceptorService,
    ILogger<UserAuditInterceptor> logger)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        try
        {
            UpdateEntities(eventData.Context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user audit records.");
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        try
        {
            UpdateEntities(eventData.Context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user audit records.");
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var auditRecords = new List<AuditRecord>();
        var auditEntries = context.ChangeTracker.Entries().Where(e => e.Entity is User);

        foreach (var entry in auditEntries)
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
            {
                continue;
            }

            var originalValue = auditRecordInterceptorService.GetOriginalValue(entry);
            var currentValue = auditRecordInterceptorService.GetCurrentValue(entry);
            var actionType = auditRecordInterceptorService.GetActionType(entry.State);

            if (originalValue == currentValue || actionType == ActionType.None)
            {
                continue;
            }

            var fieldId = entry.Property(nameof(User.Id));
            var auditRecord = auditRecordInterceptorService.CreateAuditRecord(
                aggregateId: ((UserId)fieldId.CurrentValue!).Value,
                originalValue: originalValue,
                currentValue: currentValue,
                actionType: actionType);

            auditRecords.Add(auditRecord);
        }

        if (auditRecords.Count != 0)
        {
            context.Set<AuditRecord>().AddRange(auditRecords);
        }
    }
}
