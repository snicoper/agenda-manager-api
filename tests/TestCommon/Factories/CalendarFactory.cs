using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarFactory
{
    public static Calendar CreateCalendar(
        CalendarId? calendarId = null,
        string? name = null,
        string? description = null,
        bool? isActive = null,
        CalendarSettings? settings = null)
    {
        calendarId ??= CalendarId.Create();
        settings ??= CalendarSettingsFactory.CreateCalendarSettings();

        var calendar = Calendar.Create(
            id: calendarId,
            name: name ?? "My calendar",
            description: description ?? "Description of my calendar",
            calendarSettings: settings,
            active: isActive ?? true);

        return calendar;
    }
}
