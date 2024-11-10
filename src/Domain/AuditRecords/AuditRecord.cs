using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.Events;
using AgendaManager.Domain.AuditRecords.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.AuditRecords;

public class AuditRecord : AggregateRoot
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
        ActionType actionType)
    {
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

    public string AggregateId { get; private set; } = default!;

    public string NamespaceName { get; private set; } = default!;

    public string AggregateName { get; private set; } = default!;

    public string PropertyName { get; private set; } = default!;

    public string OldValue { get; private set; } = default!;

    public string NewValue { get; private set; } = default!;

    public ActionType ActionType { get; set; }

    public static AuditRecord Create(
        AuditRecordId id,
        string aggregateId,
        string namespaceName,
        string aggregateName,
        string propertyName,
        string oldValue,
        string newValue,
        ActionType actionType)
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
}
