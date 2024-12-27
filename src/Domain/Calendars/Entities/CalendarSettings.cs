using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarSettings : AuditableEntity
{
    private CalendarSettings()
    {
    }

    private CalendarSettings(
        CalendarSettingsId calendarSettingsId,
        CalendarId calendarId,
        IanaTimeZone timeZone,
        AppointmentConfirmationRequirementStrategy appointmentConfirmationRequirement,
        AppointmentOverlappingStrategy appointmentOverlapping,
        HolidayConflictStrategy holidayConflict,
        ResourceScheduleValidationStrategy resourceScheduleValidation)
    {
        Id = calendarSettingsId;
        CalendarId = calendarId;
        TimeZone = timeZone;
        AppointmentConfirmationRequirement = appointmentConfirmationRequirement;
        AppointmentOverlapping = appointmentOverlapping;
        HolidayConflict = holidayConflict;
        ResourceScheduleValidation = resourceScheduleValidation;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public IanaTimeZone TimeZone { get; } = null!;

    public AppointmentConfirmationRequirementStrategy AppointmentConfirmationRequirement { get; }

    public AppointmentOverlappingStrategy AppointmentOverlapping { get; }

    public HolidayConflictStrategy HolidayConflict { get; }

    public ResourceScheduleValidationStrategy ResourceScheduleValidation { get; }

    internal static CalendarSettings Create(
        CalendarSettingsId calendarSettingsId,
        CalendarId calendarId,
        IanaTimeZone timeZone,
        AppointmentConfirmationRequirementStrategy appointmentConfirmationRequirement,
        AppointmentOverlappingStrategy appointmentOverlapping,
        HolidayConflictStrategy holidayConflict,
        ResourceScheduleValidationStrategy resourceScheduleValidation)
    {
        CalendarSettings settings = new(
            calendarSettingsId: calendarSettingsId,
            calendarId: calendarId,
            timeZone: timeZone,
            appointmentConfirmationRequirement: appointmentConfirmationRequirement,
            appointmentOverlapping: appointmentOverlapping,
            holidayConflict: holidayConflict,
            resourceScheduleValidation: resourceScheduleValidation);

        settings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(calendarSettingsId));

        return settings;
    }

    internal bool HasChanges(CalendarSettings settings)
    {
        return TimeZone != settings.TimeZone ||
            AppointmentConfirmationRequirement != settings.AppointmentConfirmationRequirement ||
            AppointmentOverlapping != settings.AppointmentOverlapping ||
            HolidayConflict != settings.HolidayConflict ||
            ResourceScheduleValidation != settings.ResourceScheduleValidation;
    }
}
