using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarSettingsManager(ICalendarRepository calendarRepository)
{
    public async Task<Result> UpdateCalendarSettings(
        CalendarId calendarId,
        IanaTimeZone timeZone,
        AppointmentConfirmationRequirementStrategy appointmentConfirmation,
        AppointmentOverlappingStrategy appointmentOverlapping,
        HolidayConflictStrategy holidayConflict,
        ResourceScheduleValidationStrategy resourceSchedulesAvailability)
    {
        var calendar = await calendarRepository.GetByIdWithSettingsAsync(calendarId);

        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        var calendarSettings = CalendarSettings.Create(
            calendarSettingsId: calendar.SettingsId,
            calendarId: calendarId,
            timeZone: timeZone,
            appointmentConfirmationRequirement: appointmentConfirmation,
            appointmentOverlapping: appointmentOverlapping,
            holidayConflict: holidayConflict,
            resourceScheduleValidation: resourceSchedulesAvailability);

        var hasChanged = calendar.UpdateSettings(calendarSettings);

        if (!hasChanged)
        {
            calendarRepository.Update(calendar);
        }

        return Result.Success();
    }
}
