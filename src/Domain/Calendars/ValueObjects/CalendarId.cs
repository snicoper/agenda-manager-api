namespace AgendaManager.Domain.Calendars.ValueObjects;

public sealed record CalendarId
{
    private CalendarId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static CalendarId From(Guid value)
    {
        return new CalendarId(value);
    }

    public static CalendarId Create()
    {
        return new CalendarId(Guid.NewGuid());
    }
}
