using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.CalendarEvents.ValueObjects;

public class CalendarEventId : ValueObject
{
    private CalendarEventId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CalendarEventId From(Guid value)
    {
        return new CalendarEventId(value);
    }

    public static CalendarEventId Create()
    {
        return new CalendarEventId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
