using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.AuditRecords.ValueObjects;

public sealed class AuditRecordId : ValueObject
{
    private AuditRecordId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static AuditRecordId From(Guid value)
    {
        return new AuditRecordId(value);
    }

    public static AuditRecordId Create()
    {
        return new AuditRecordId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
