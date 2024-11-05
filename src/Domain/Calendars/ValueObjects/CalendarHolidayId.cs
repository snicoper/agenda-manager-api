using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars.ValueObjects;

public class CalendarHolidayId : ValueObject
{
    public CalendarHolidayId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CalendarHolidayId From(Guid value)
    {
        return new CalendarHolidayId(value);
    }

    public static CalendarHolidayId Create()
    {
        return new CalendarHolidayId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
