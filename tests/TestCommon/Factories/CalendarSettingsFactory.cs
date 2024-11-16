using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarSettingsFactory
{
    public static CalendarSettings CreateCalendarSettings(
        CalendarSettingsId? calendarSettingsId = null,
        CalendarId? calendarId = null,
        IanaTimeZone? timeZone = null,
        HolidayCreateStrategy? holidayCreateStrategy = null,
        AppointmentOverlappingStrategy? appointmentOverlappingStrategy = null)
    {
        var settings = CalendarSettings.Create(
            id: calendarSettingsId ?? CalendarSettingsId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            ianaTimeZone: timeZone ?? IanaTimeZone.FromIana(IanaTimeZoneConstants.EuropeMadrid),
            holidayCreateStrategy ?? HolidayCreateStrategy.RejectIfOverlapping,
            appointmentOverlappingStrategy ?? AppointmentOverlappingStrategy.RejectIfOverlapping);

        return settings;
    }
}
