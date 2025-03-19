using Newtonsoft.Json;

namespace AgendaManager.Domain.Calendars.ValueObjects;

public sealed record CalendarHolidayId
{
    [JsonConstructor]
    internal CalendarHolidayId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

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
}
