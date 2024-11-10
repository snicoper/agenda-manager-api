using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.AuditRecords.Enums;
using AgendaManager.Domain.AuditRecords.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class AuditRecordFactory
{
    public static AuditRecord Create(
        AuditRecordId? auditRecordId = null,
        string? aggregateId = null,
        string? namespaceName = null,
        string? aggregateName = null,
        string? propertyName = null,
        string? oldValue = null,
        string? newValue = null,
        ActionType? actionType = null)
    {
        var auditRecord = AuditRecord.Create(
            id: auditRecordId ?? AuditRecordId.Create(),
            aggregateId: aggregateId ?? "Test Aggregate Id",
            namespaceName: namespaceName ?? "Test Namespace",
            aggregateName: aggregateName ?? "Test Aggregate Name",
            propertyName: propertyName ?? "Test Property Name",
            oldValue: oldValue ?? "Test Old Value",
            newValue: newValue ?? "Test New Value",
            actionType: actionType ?? ActionType.Create);

        return auditRecord;
    }
}
