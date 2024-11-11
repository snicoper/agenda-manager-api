using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars.ValueObjects;

public class CalendarId : ValueObject
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
