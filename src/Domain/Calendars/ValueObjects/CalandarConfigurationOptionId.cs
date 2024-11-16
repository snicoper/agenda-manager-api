namespace AgendaManager.Domain.Calendars.ValueObjects;

public record CalandarConfigurationOptionId
{
    private CalandarConfigurationOptionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CalandarConfigurationOptionId From(Guid value)
    {
        return new CalandarConfigurationOptionId(value);
    }

    public static CalandarConfigurationOptionId Create()
    {
        return new CalandarConfigurationOptionId(Guid.NewGuid());
    }
}
