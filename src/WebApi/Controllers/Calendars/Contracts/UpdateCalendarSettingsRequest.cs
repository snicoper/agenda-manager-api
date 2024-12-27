using AgendaManager.Domain.Calendars.Enums;

namespace AgendaManager.WebApi.Controllers.Calendars.Contracts;

public record UpdateCalendarSettingsRequest(
    string TimeZone,
    AppointmentConfirmationRequirementStrategy AppointmentConfirmationRequirement,
    AppointmentOverlappingStrategy AppointmentOverlapping,
    HolidayConflictStrategy HolidayConflict,
    ResourceScheduleValidationStrategy ResourceScheduleValidation);
