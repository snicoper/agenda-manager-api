using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.ResourceAvailabilities.ValueObjects;

public class ResourceAvailabilityId : ValueObject
{
    private ResourceAvailabilityId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ResourceAvailabilityId From(Guid value)
    {
        return new ResourceAvailabilityId(value);
    }

    public static ResourceAvailabilityId Create()
    {
        return new ResourceAvailabilityId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
