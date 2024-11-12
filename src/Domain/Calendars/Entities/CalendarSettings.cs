using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Events;
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
        HolidayCreationStrategy holidayCreationStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayCreationStrategy);

        Id = id;
        CalendarId = calendarId;
        IanaTimeZone = ianaTimeZone;
        HolidayCreationStrategy = holidayCreationStrategy;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public IanaTimeZone IanaTimeZone { get; private set; } = default!;

    public HolidayCreationStrategy HolidayCreationStrategy { get; private set; }

    internal static CalendarSettings Create(
        CalendarSettingsId id,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        HolidayCreationStrategy holidayCreationStrategy)
    {
        CalendarSettings calendarSettings = new(id, calendarId, ianaTimeZone, holidayCreationStrategy);

        calendarSettings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(id));

        return calendarSettings;
    }

    internal void Update(IanaTimeZone ianaTimeZone, HolidayCreationStrategy holidayCreationStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayCreationStrategy);

        if (!HasChanges(ianaTimeZone, holidayCreationStrategy))
        {
            return;
        }

        IanaTimeZone = ianaTimeZone;
        HolidayCreationStrategy = holidayCreationStrategy;

        AddDomainEvent(new CalendarSettingsUpdatedDomainEvent(Id, CalendarId));
    }

    internal bool HasChanges(IanaTimeZone ianaTimeZone, HolidayCreationStrategy holidayCreationStrategy)
    {
        return !IanaTimeZone.Equals(ianaTimeZone) || HolidayCreationStrategy != holidayCreationStrategy;
    }
}
