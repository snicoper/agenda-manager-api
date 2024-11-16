using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
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
        IanaTimeZone? timeZone = null,
        HolidayStrategy? holidayCreationStrategy = null,
        bool? isActive = null,
        CalendarSettings? settings = null)
    {
        calendarId ??= CalendarId.Create();

        settings ??= CalendarSettingsFactory.CreateCalendarSettings(
            calendarId: calendarId,
            timeZone: timeZone ?? IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
            holidayStrategy: holidayCreationStrategy ?? HolidayStrategy.RejectIfOverlapping,
            appointmentStrategy: AppointmentStrategy.RejectIfOverlapping);

        var calendar = Calendar.Create(
            id: calendarId,
            name: name ?? "My calendar",
            description: description ?? "Description of my calendar",
            calendarSettings: settings,
            active: isActive ?? true);

        return calendar;
    }
}
