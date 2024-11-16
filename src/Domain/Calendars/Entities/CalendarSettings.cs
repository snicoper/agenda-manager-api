using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Models;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarSettings : AuditableEntity
{
    private CalendarSettings()
    {
    }

    private CalendarSettings(
        CalendarSettingsId id,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        HolidayCreateStrategy holidayCreateStrategy,
        AppointmentOverlappingStrategy appointmentOverlappingStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayCreateStrategy);

        Id = id;
        CalendarId = calendarId;
        IanaTimeZone = ianaTimeZone;
        HolidayCreateStrategy = holidayCreateStrategy;
        AppointmentOverlappingStrategy = appointmentOverlappingStrategy;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public IanaTimeZone IanaTimeZone { get; private set; } = default!;

    public HolidayCreateStrategy HolidayCreateStrategy { get; private set; }

    public AppointmentOverlappingStrategy AppointmentOverlappingStrategy { get; private set; }

    public AppointmentCreationStrategy AppointmentCreationStrategy { get; private set; }

    internal static CalendarSettings Create(
        CalendarSettingsId id,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        HolidayCreateStrategy holidayCreateStrategy,
        AppointmentOverlappingStrategy appointmentOverlappingStrategy)
    {
        CalendarSettings calendarSettings = new(
            id,
            calendarId,
            ianaTimeZone,
            holidayCreateStrategy,
            appointmentOverlappingStrategy);

        calendarSettings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(id));

        return calendarSettings;
    }

    internal void Update(CalendarSettingsConfiguration settings)
    {
        ArgumentNullException.ThrowIfNull(settings.HolidayCreateStrategy);
        ArgumentNullException.ThrowIfNull(settings.AppointmentOverlappingStrategy);

        if (!HasChanges(settings))
        {
            return;
        }

        IanaTimeZone = settings.IanaTimeZone;
        HolidayCreateStrategy = settings.HolidayCreateStrategy;
        AppointmentOverlappingStrategy = settings.AppointmentOverlappingStrategy;
    }

    internal bool HasChanges(CalendarSettingsConfiguration settings)
    {
        return !(IanaTimeZone.Equals(settings.IanaTimeZone)
                 && HolidayCreateStrategy == settings.HolidayCreateStrategy
                 && AppointmentOverlappingStrategy == settings.AppointmentOverlappingStrategy);
    }
}
