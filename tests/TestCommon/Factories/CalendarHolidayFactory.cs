using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class CalendarHolidayFactory
{
    public static CalendarHoliday CreateCalendarHoliday(
        CalendarHolidayId? calendarHolidayId = null,
        CalendarId? calendarId = null,
        Period? period = null,
        string? name = null)
    {
        var calendarHoliday = CalendarHoliday.Create(
            calendarHolidayId: calendarHolidayId ?? CalendarHolidayId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            period: period ?? Period.From(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1)),
            name: name ?? "Holiday Name");

        return calendarHoliday;
    }
}
