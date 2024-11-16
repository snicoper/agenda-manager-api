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
        AppointmentStrategy appointmentStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayStrategy);

        Id = id;
        CalendarId = calendarId;
        IanaTimeZone = ianaTimeZone;
        HolidayStrategy = holidayStrategy;
        AppointmentStrategy = appointmentStrategy;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public IanaTimeZone IanaTimeZone { get; private set; } = default!;

    public HolidayStrategy HolidayStrategy { get; private set; }

    public AppointmentStrategy AppointmentStrategy { get; private set; }

    internal static CalendarSettings Create(
        CalendarSettingsId id,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        HolidayStrategy holidayStrategy,
        AppointmentStrategy appointmentStrategy)
    {
        CalendarSettings calendarSettings = new(
            id,
            calendarId,
            ianaTimeZone,
            holidayStrategy,
            appointmentStrategy);

        calendarSettings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(id));

        return calendarSettings;
    }

    internal void Update(CalendarSettingsConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration.HolidayStrategy);
        ArgumentNullException.ThrowIfNull(configuration.AppointmentStrategy);

        if (!HasChanges(configuration))
        {
            return;
        }

        IanaTimeZone = configuration.IanaTimeZone;
        HolidayStrategy = configuration.HolidayStrategy;
        AppointmentStrategy = configuration.AppointmentStrategy;
    }

    internal bool HasChanges(CalendarSettingsConfiguration configuration)
    {
        return !(IanaTimeZone.Equals(configuration.IanaTimeZone)
                 && HolidayStrategy == configuration.HolidayStrategy
                 && AppointmentStrategy == configuration.AppointmentStrategy);
    }
}
