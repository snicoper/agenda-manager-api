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
        HolidayStrategy holidayStrategy,
        AppointmentOverlappingStrategy appointmentOverlappingStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayStrategy);

        Id = id;
        CalendarId = calendarId;
        IanaTimeZone = ianaTimeZone;
        HolidayStrategy = holidayStrategy;
        AppointmentOverlappingStrategy = appointmentOverlappingStrategy;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public IanaTimeZone IanaTimeZone { get; private set; } = default!;

    public HolidayStrategy HolidayStrategy { get; private set; }

    public AppointmentOverlappingStrategy AppointmentOverlappingStrategy { get; private set; }

    internal static CalendarSettings Create(
        CalendarSettingsId id,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        HolidayStrategy holidayStrategy,
        AppointmentOverlappingStrategy appointmentOverlappingStrategy)
    {
        CalendarSettings calendarSettings = new(
            id,
            calendarId,
            ianaTimeZone,
            holidayStrategy,
            appointmentOverlappingStrategy);

        calendarSettings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(id));

        return calendarSettings;
    }

    internal void Update(CalendarSettingsConfiguration settings)
    {
        ArgumentNullException.ThrowIfNull(settings.HolidayStrategy);
        ArgumentNullException.ThrowIfNull(settings.AppointmentOverlappingStrategy);

        if (!HasChanges(settings))
        {
            return;
        }

        IanaTimeZone = settings.IanaTimeZone;
        HolidayStrategy = settings.HolidayStrategy;
        AppointmentOverlappingStrategy = settings.AppointmentOverlappingStrategy;
    }

    internal bool HasChanges(CalendarSettingsConfiguration settings)
    {
        return !(IanaTimeZone.Equals(settings.IanaTimeZone)
                 && HolidayStrategy == settings.HolidayStrategy
                 && AppointmentOverlappingStrategy == settings.AppointmentOverlappingStrategy);
    }
}
