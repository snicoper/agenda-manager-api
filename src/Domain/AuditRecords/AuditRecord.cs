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
        Guid aggregateId,
        string aggregateName,
        string fieldName,
        string oldValue,
        string newValue,
        ActionType actionType)
    {
        Id = id;
        AggregateId = aggregateId;
        AggregateName = aggregateName;
        FieldName = fieldName;
        OldValue = oldValue;
        NewValue = newValue;
        ActionType = actionType;
    }

    public AuditRecordId Id { get; } = null!;

    public Guid AggregateId { get; private set; }

    public string AggregateName { get; private set; } = default!;

    public string FieldName { get; private set; } = default!;

    public string OldValue { get; private set; } = default!;

    public string NewValue { get; private set; } = default!;

    public ActionType ActionType { get; set; }

    public static AuditRecord Create(
        AuditRecordId id,
        Guid aggregateId,
        string aggregateName,
        string fieldName,
        string oldValue,
        string newValue,
        ActionType actionType)
    {
        AuditRecord auditRecord = new(id, aggregateId, aggregateName, fieldName, oldValue, newValue, actionType);

        auditRecord.AddDomainEvent(new AuditRecordCreatedDomainEvent(id));

        return auditRecord;
    }
}
