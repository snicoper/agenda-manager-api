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

    public void RecordAuditEntries<TAggregate>(DbContext? context, string entityId, List<string> auditableProperties)
        where TAggregate : IEntity
    {
        if (context is null || auditableProperties.Count == 0)
        {
            return;
        }

        try
        {
            _entityType = typeof(TAggregate);

            var auditEntries = context.ChangeTracker.Entries()
                .Where(
                    e => e is
                    {
                        Entity: TAggregate,
                        State: EntityState.Added or EntityState.Modified or EntityState.Deleted
                    });

            var auditRecords = auditEntries.SelectMany(
                    entry => auditableProperties.Select(
                        propertyName => CreateAuditRecordIfChanged(entry, propertyName, entityId)))
                .Where(record => record is not null)
                .Cast<AuditRecord>()
                .ToList();

            if (auditRecords.Count != 0)
            {
                context.Set<AuditRecord>().AddRange(auditRecords);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error auditing {EntityName} with ID {EntityId}", _entityType?.Name, entityId);
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

    private AuditRecord? CreateAuditRecordIfChanged(EntityEntry entry, string propertyName, string entityId)
    {
        var originalValue = GetOriginalValue(entry, propertyName);
        var currentValue = GetCurrentValue(entry, propertyName);
        var actionType = GetActionType(entry.State);

        if (originalValue == currentValue || actionType == ActionType.None)
        {
            return null;
        }

        var auditRecordResult = CreateAuditRecord(
            aggregateId: GetAggregateIdValue(entry, entityId) ?? string.Empty,
            propertyName: propertyName,
            originalValue: originalValue,
            currentValue: currentValue,
            actionType: actionType);

        return auditRecordResult;
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
