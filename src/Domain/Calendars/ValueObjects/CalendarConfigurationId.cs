namespace AgendaManager.Domain.Calendars.ValueObjects;

public record CalendarConfigurationId
{
    private CalendarConfigurationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }

    public static CalendarConfigurationId From(Guid value)
    {
        return new CalendarConfigurationId(value);
    }

    public static CalendarConfigurationId Create()
    {
        return new CalendarConfigurationId(Guid.NewGuid());
    }
}
