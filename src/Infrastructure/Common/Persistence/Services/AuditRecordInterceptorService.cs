using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Persistence.Services;

public class AuditRecordInterceptorService(
    ICurrentUserProvider currentUserProvider,
    IDateTimeProvider dateTimeProvider,
    ILogger<AuditRecordInterceptorService> logger)
{
    private Type? _entityType;

    public void UpdateEntities<TAggregate>(DbContext? context, string entityId, List<string> propertyNames)
        where TAggregate : IEntity
    {
        if (context is null)
        {
            return;
        }

        try
        {
            _entityType = typeof(TAggregate);

            var auditRecords = new List<AuditRecord>();
            var auditEntries = context.ChangeTracker.Entries().Where(e => e.Entity is TAggregate);

            foreach (var entry in auditEntries)
            {
                if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted))
                {
                    continue;
                }

                foreach (var propertyName in propertyNames)
                {
                    var originalValue = GetOriginalValue(entry, propertyName);
                    var currentValue = GetCurrentValue(entry, propertyName);
                    var actionType = GetActionType(entry.State);

                    if (originalValue == currentValue || actionType == ActionType.None)
                    {
                        continue;
                    }

                    var auditRecord = CreateAuditRecord(
                        aggregateId: GetAggregateIdValue(entry, entityId) ?? string.Empty,
                        propertyName: propertyName,
                        originalValue: originalValue,
                        currentValue: currentValue,
                        actionType: actionType);

                    auditRecords.Add(auditRecord);
                }
            }

            if (auditRecords.Count != 0)
            {
                context.Set<AuditRecord>().AddRange(auditRecords);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating role audit records.");
        }
    }

    private static string? GetAggregateIdValue(EntityEntry entry, string entityId)
    {
        var property = entry.Property(entityId);
        var currentValue = property.CurrentValue;

        return currentValue?.GetType().GetProperty("Value") is not null
            ? ValueObjectHelper.GetValueFromValueObject(currentValue).ToString()
            : currentValue?.ToString();
    }

    private static string GetOriginalValue(EntityEntry entry, string propertyName)
    {
        return entry.State == EntityState.Added
            ? string.Empty
            : entry.Property(propertyName).OriginalValue?.ToString() ?? string.Empty;
    }

    private static string GetCurrentValue(EntityEntry entry, string propertyName)
    {
        return entry.State == EntityState.Deleted
            ? string.Empty
            : entry.Property(propertyName).CurrentValue?.ToString() ?? string.Empty;
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

    private AuditRecord CreateAuditRecord(
        string aggregateId,
        string propertyName,
        string originalValue,
        string currentValue,
        ActionType actionType)
    {
        var currentDateTime = dateTimeProvider.UtcNow;
        var currentUser = currentUserProvider.GetCurrentUser();
        var currentUserId = currentUser?.Id.Value.ToString() ?? "System";

        var auditRecord = AuditRecord.Create(
            id: AuditRecordId.Create(),
            aggregateId: aggregateId,
            namespaceName: _entityType?.Namespace ?? string.Empty,
            aggregateName: _entityType?.Name ?? string.Empty,
            propertyName: propertyName,
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
