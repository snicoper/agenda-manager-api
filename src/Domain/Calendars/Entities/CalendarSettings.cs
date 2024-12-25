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
        AppointmentConfirmationRequirementStrategy confirmationRequirement,
        AppointmentOverlappingStrategy overlapBehavior,
        HolidayConflictStrategy holidayAppointmentHandling,
        ResourceScheduleValidationStrategy scheduleValidation)
    {
        Id = calendarSettingsId;
        CalendarId = calendarId;
        TimeZone = timeZone;
        ConfirmationRequirement = confirmationRequirement;
        OverlapBehavior = overlapBehavior;
        HolidayAppointmentHandling = holidayAppointmentHandling;
        ScheduleValidation = scheduleValidation;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public IanaTimeZone TimeZone { get; } = null!;

    public AppointmentConfirmationRequirementStrategy ConfirmationRequirement { get; }

    public AppointmentOverlappingStrategy OverlapBehavior { get; }

    public HolidayConflictStrategy HolidayAppointmentHandling { get; }

    public ResourceScheduleValidationStrategy ScheduleValidation { get; }

    internal static CalendarSettings Create(
        CalendarSettingsId calendarSettingsId,
        CalendarId calendarId,
        IanaTimeZone timeZone,
        AppointmentConfirmationRequirementStrategy confirmationRequirement,
        AppointmentOverlappingStrategy overlapBehavior,
        HolidayConflictStrategy holidayAppointmentHandling,
        ResourceScheduleValidationStrategy scheduleValidation)
    {
        CalendarSettings settings = new(
            calendarSettingsId: calendarSettingsId,
            calendarId: calendarId,
            timeZone: timeZone,
            confirmationRequirement: confirmationRequirement,
            overlapBehavior: overlapBehavior,
            holidayAppointmentHandling: holidayAppointmentHandling,
            scheduleValidation: scheduleValidation);

        settings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(calendarSettingsId));

        return settings;
    }

    internal bool HasChanges(CalendarSettings settings)
    {
        return TimeZone != settings.TimeZone ||
            ConfirmationRequirement != settings.ConfirmationRequirement ||
            OverlapBehavior != settings.OverlapBehavior ||
            HolidayAppointmentHandling != settings.HolidayAppointmentHandling ||
            ScheduleValidation != settings.ScheduleValidation;
    }
}
