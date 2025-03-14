﻿using AgendaManager.Domain.Calendars.Enums;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarSettings;

public record GetCalendarSettingsQueryResponse(
    Guid CalendarId,
    string TimeZone,
    AppointmentConfirmationRequirementStrategy AppointmentConfirmationRequirement,
    AppointmentOverlappingStrategy AppointmentOverlapping,
    HolidayConflictStrategy HolidayConflict,
    ResourceScheduleValidationStrategy ResourceScheduleValidation);
