using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarFactory
{
    public static Calendar CreateCalendar(
        CalendarId? calendarId = null,
        string? name = null,
        string? description = null,
        string? timeZone = null,
        HolidayCreationStrategy? holidayCreationStrategy = null,
        bool? isActive = null)
    {
        calendarId ??= CalendarId.Create();

        var calendar = Calendar.Create(
            id: calendarId,
            name: name ?? "My calendar",
            description: description ?? "Description of my calendar",
            timeZone: timeZone ?? TimeZoneConstants.EuropeMadrid,
            holidayCreationStrategy: holidayCreationStrategy ?? HolidayCreationStrategy.CancelOverlapping,
            active: isActive ?? true);

        return calendar;
    }
}
