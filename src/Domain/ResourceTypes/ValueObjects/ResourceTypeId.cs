using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.ResourceTypes.ValueObjects;

public class ResourceTypeId : ValueObject
{
    private ResourceTypeId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static ResourceTypeId From(Guid value)
    {
        return new ResourceTypeId(value);
    }

    public static ResourceTypeId Create()
    {
        return new ResourceTypeId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
