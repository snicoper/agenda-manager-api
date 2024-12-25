using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarFactory
{
    public static Calendar CreateCalendar(
        CalendarId? calendarId = null,
        CalendarSettings? settings = null,
        string? name = null,
        string? description = null,
        bool? isActive = null)
    {
        calendarId ??= CalendarId.Create();

        var calendar = Calendar.Create(
            id: calendarId,
            settings: settings ?? CalendarSettingsFactory.CreateCalendarSettings(),
            name: name ?? "My calendar",
            description: description ?? "Description of my calendar",
            isActive: isActive ?? true);

        return calendar;
    }
}
