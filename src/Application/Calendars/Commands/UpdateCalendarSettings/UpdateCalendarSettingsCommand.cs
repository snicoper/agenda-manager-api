using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Calendars.Enums;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendarSettings;

[Authorize(Permissions = SystemPermissions.CalendarSettings.Update)]
public record UpdateCalendarSettingsCommand(
    Guid CalendarId,
    string TimeZone,
    AppointmentConfirmationRequirementStrategy AppointmentConfirmationRequirement,
    AppointmentOverlappingStrategy AppointmentOverlapping,
    HolidayConflictStrategy HolidayConflict,
    ResourceScheduleValidationStrategy ResourceScheduleValidation) : ICommand;
