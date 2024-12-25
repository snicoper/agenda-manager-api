namespace AgendaManager.Domain.Calendars.ValueObjects;

public record CalendarSettingsId
{
    private CalendarSettingsId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static CalendarSettingsId From(Guid value)
    {
        return new CalendarSettingsId(value);
    }

    public static CalendarSettingsId Create()
    {
        return new CalendarSettingsId(Guid.NewGuid());
    }
}
