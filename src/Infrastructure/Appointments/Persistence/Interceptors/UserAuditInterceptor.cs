using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Interceptors;

public class UserAuditInterceptor(
    ICurrentUserProvider currentUserProvider,
    IDateTimeProvider dateTimeProvider,
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

    private static string GetOriginalValue(EntityEntry entry)
    {
        return entry.State == EntityState.Added
            ? string.Empty
            : entry.Property(nameof(User.Active)).OriginalValue?.ToString() ?? string.Empty;
    }

    private static string GetCurrentValue(EntityEntry entry)
    {
        return entry.State == EntityState.Deleted
            ? string.Empty
            : entry.Property(nameof(User.Active)).CurrentValue?.ToString() ?? string.Empty;
    }

    private static ActionType GetActionType(EntityState state)
    {
        return state switch
        {
            EntityState.Added => ActionType.Create,
            EntityState.Modified => ActionType.Update,
            EntityState.Deleted => ActionType.Delete,
            EntityState.Detached => ActionType.None,
            EntityState.Unchanged => ActionType.None,
            _ => throw new ArgumentOutOfRangeException()
        };
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

            var originalValue = GetOriginalValue(entry);
            var currentValue = GetCurrentValue(entry);
            var actionType = GetActionType(entry.State);

            var auditRecord = CreateAuditRecord(entry, originalValue, currentValue, actionType);
            auditRecords.Add(auditRecord);
        }

        if (auditRecords.Count != 0)
        {
            context.Set<AuditRecord>().AddRange(auditRecords);
        }
    }

    private AuditRecord CreateAuditRecord(
        EntityEntry entry,
        string originalValue,
        string currentValue,
        ActionType actionType)
    {
        var fieldId = entry.Property(nameof(User.Id));
        var currentDateTime = dateTimeProvider.UtcNow;
        var currentUser = currentUserProvider.GetCurrentUser();
        var currentUserId = currentUser?.Id.Value.ToString() ?? "System";

        var auditRecord = AuditRecord.Create(
            id: AuditRecordId.Create(),
            aggregateId: ((UserId)fieldId.CurrentValue!).Value,
            aggregateName: nameof(User),
            fieldName: nameof(User.Active),
            oldValue: originalValue,
            newValue: currentValue,
            actionType: actionType);

        auditRecord.CreatedBy = currentUserId;
        auditRecord.CreatedAt = currentDateTime;
        auditRecord.LastModifiedBy = currentUserId;
        auditRecord.LastModifiedAt = currentDateTime;
        auditRecord.Version = 1;

        return auditRecord;
    }
}
