using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Resources.ValueObjects;

public sealed class ResourceId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;

    public static ResourceId From(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new ResourceId(value);
    }

    public static ResourceId Create()
    {
        return new ResourceId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
