using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Resources.ValueObjects;

public class ResourceTypeId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;

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
