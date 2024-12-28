namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

public sealed record ResourceTypeId
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
}
