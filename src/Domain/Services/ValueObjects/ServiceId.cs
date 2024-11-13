namespace AgendaManager.Domain.Services.ValueObjects;

public sealed record ServiceId
{
    private ServiceId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static ServiceId From(Guid value)
    {
        return new ServiceId(value);
    }

    public static ServiceId Create()
    {
        return new ServiceId(Guid.NewGuid());
    }
}
