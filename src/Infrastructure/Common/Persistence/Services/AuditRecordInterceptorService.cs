using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AgendaManager.Infrastructure.Common.Persistence.Services;

public class AuditRecordInterceptorService(ICurrentUserProvider currentUserProvider, IDateTimeProvider dateTimeProvider)
{
    public AuditRecord CreateAuditRecord<TAggregate>(
        Guid aggregateId,
        string propertyName,
        string originalValue,
        string currentValue,
        ActionType actionType)
    {
        var namespaceName = $"{typeof(TAggregate).Namespace}{typeof(TAggregate).Name}";
        var aggregateName = typeof(TAggregate).Name;

        var currentDateTime = dateTimeProvider.UtcNow;
        var currentUser = currentUserProvider.GetCurrentUser();
        var currentUserId = currentUser?.Id.Value.ToString() ?? "System";

        var auditRecord = AuditRecord.Create(
            id: AuditRecordId.Create(),
            aggregateId: aggregateId,
            namespaceName: namespaceName,
            aggregateName: aggregateName,
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

    public string GetOriginalValue(EntityEntry entry, string propertyName)
    {
        return entry.State == EntityState.Added
            ? string.Empty
            : entry.Property(propertyName).OriginalValue?.ToString() ?? string.Empty;
    }

    public string GetCurrentValue(EntityEntry entry, string propertyName)
    {
        return entry.State == EntityState.Deleted
            ? string.Empty
            : entry.Property(propertyName).CurrentValue?.ToString() ?? string.Empty;
    }

    public ActionType GetActionType(EntityState state)
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
}
