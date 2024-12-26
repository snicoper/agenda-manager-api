using AgendaManager.Domain.Calendars.Enums;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarSettings;

public record GetCalendarSettingsQueryResponse(
    Guid CalendarId,
    string TimeZone,
    AppointmentConfirmationRequirementStrategy ConfirmationRequirement,
    AppointmentOverlappingStrategy OverlapBehavior,
    HolidayConflictStrategy HolidayAppointmentHandling,
    ResourceScheduleValidationStrategy ScheduleValidation);
