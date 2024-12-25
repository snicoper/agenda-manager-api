using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarSettingsFactory
{
    public static CalendarSettings CreateCalendarSettings(
        CalendarSettingsId? calendarSettingsId = null,
        CalendarId? calendarId = null,
        IanaTimeZone? timeZone = null,
        AppointmentConfirmationRequirementStrategy? appointmentConfirmation = null,
        AppointmentOverlappingStrategy? appointmentOverlapping = null,
        HolidayConflictStrategy? holidayConflict = null,
        ResourceScheduleValidationStrategy? resourceSchedulesAvailability = null)
    {
        var calendarSettings = CalendarSettings.Create(
            calendarSettingsId ?? CalendarSettingsId.Create(),
            calendarId ?? CalendarId.Create(),
            timeZone ?? IanaTimeZone.FromIana("Europe/Madrid"),
            appointmentConfirmation ?? AppointmentConfirmationRequirementStrategy.Require,
            appointmentOverlapping ?? AppointmentOverlappingStrategy.Allow,
            holidayConflict ?? HolidayConflictStrategy.Allow,
            resourceSchedulesAvailability ?? ResourceScheduleValidationStrategy.Validate);

        return calendarSettings;
    }
}
