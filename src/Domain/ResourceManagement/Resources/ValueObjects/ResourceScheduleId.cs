namespace AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

public sealed record ResourceScheduleId
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
}
