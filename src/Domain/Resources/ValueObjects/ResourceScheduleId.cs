using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Resources.ValueObjects;

public class ResourceScheduleId : ValueObject
{
    private ResourceScheduleId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static ResourceScheduleId From(Guid value)
    {
        return new ResourceScheduleId(value);
    }

    public static ResourceScheduleId Create()
    {
        return new ResourceScheduleId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
