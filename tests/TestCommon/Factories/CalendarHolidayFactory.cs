using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.TestCommon.Factories;

public static class CalendarHolidayFactory
{
    public static CalendarHoliday CreateCalendarHoliday(
        CalendarHolidayId? calendarHolidayId = null,
        CalendarId? calendarId = null,
        Period? period = null,
        WeekDays? weekDays = null,
        string? name = null,
        string? description = null)
    {
        var calendarHoliday = CalendarHoliday.Create(
            calendarHolidayId: calendarHolidayId ?? CalendarHolidayId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            period: period ?? Period.From(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1)),
            weekDays: weekDays ?? WeekDays.Weekdays,
            name: name ?? "Holiday Name",
            description: description ?? "Holiday Description");

        return calendarHoliday;
    }
}
