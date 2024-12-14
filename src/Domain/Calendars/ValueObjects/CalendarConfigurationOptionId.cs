namespace AgendaManager.Domain.Calendars.ValueObjects;

public record CalendarConfigurationOptionId
{
    private CalendarConfigurationOptionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CalendarConfigurationOptionId From(Guid value)
    {
        return new CalendarConfigurationOptionId(value);
    }

    public static CalendarConfigurationOptionId Create()
    {
        return new CalendarConfigurationOptionId(Guid.NewGuid());
    }
}
