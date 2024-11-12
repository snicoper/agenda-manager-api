using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarFactory
{
    public static Calendar CreateCalendar(
        CalendarId? calendarId = null,
        string? name = null,
        string? description = null,
        bool? isActive = null)
    {
        calendarId ??= CalendarId.Create();

        var settings = CalendarSettings.Create(
            id: CalendarSettingsId.Create(),
            calendarId: calendarId,
            timeZone: "Europe/Madrid",
            HolidayCreationStrategy.CancelOverlapping);

        var calendar = Calendar.Create(
            id: calendarId,
            settings: settings,
            name: name ?? "My calendar",
            description: description ?? "Description of my calendar",
            active: isActive ?? true);

        return calendar;
    }
}
