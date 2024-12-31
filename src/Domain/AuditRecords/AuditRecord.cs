using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.Events;
using AgendaManager.Domain.AuditRecords.Exceptions;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.AuditRecords;

public sealed class AuditRecord : AggregateRoot
{
    private AuditRecord()
    {
    }

    private AuditRecord(
        AuditRecordId id,
        string aggregateId,
        string namespaceName,
        string aggregateName,
        string propertyName,
        string oldValue,
        string newValue,
        AuditRecordActionType actionType)
    {
        ArgumentNullException.ThrowIfNull(aggregateId);
        ArgumentNullException.ThrowIfNull(namespaceName);
        ArgumentNullException.ThrowIfNull(aggregateName);
        ArgumentNullException.ThrowIfNull(propertyName);
        ArgumentNullException.ThrowIfNull(oldValue);
        ArgumentNullException.ThrowIfNull(newValue);

        GuardAgainstInvalidActionType(actionType);

        Id = id;
        AggregateId = aggregateId;
        NamespaceName = namespaceName;
        AggregateName = aggregateName;
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
        ActionType = actionType;
    }

    public AuditRecordId Id { get; } = null!;

    public string AggregateId { get; private set; } = null!;

    public string NamespaceName { get; private set; } = null!;

    public string AggregateName { get; private set; } = null!;

    public string PropertyName { get; private set; } = null!;

    public string OldValue { get; private set; } = null!;

    public string NewValue { get; private set; } = null!;

    public AuditRecordActionType ActionType { get; private set; }

    public static AuditRecord Create(
        AuditRecordId id,
        string aggregateId,
        string namespaceName,
        string aggregateName,
        string propertyName,
        string oldValue,
        string newValue,
        AuditRecordActionType actionType)
    {
        AuditRecord auditRecord = new(
            id,
            aggregateId,
            namespaceName,
            aggregateName,
            propertyName,
            oldValue,
            newValue,
            actionType);

        auditRecord.AddDomainEvent(new AuditRecordCreatedDomainEvent(id));

        return auditRecord;
    }

    private static void GuardAgainstInvalidActionType(AuditRecordActionType actionType)
    {
        if (!Enum.IsDefined(typeof(AuditRecordActionType), actionType))
        {
            throw new AuditRecordDomainException("Invalid action type.");
        }

        if (actionType == AuditRecordActionType.None)
        {
            throw new AuditRecordDomainException("Action type cannot be None.");
        }
    }
}
