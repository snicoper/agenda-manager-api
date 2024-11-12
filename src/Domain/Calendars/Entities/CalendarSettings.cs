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

    private CalendarSettings(CalendarSettingsId id, string timeZone, HolidayCreationStrategy holidayCreationStrategy)
    {
        ArgumentNullException.ThrowIfNull(timeZone);
        ArgumentNullException.ThrowIfNull(holidayCreationStrategy);

        Id = id;
        TimeZone = timeZone;
        HolidayCreationStrategy = holidayCreationStrategy;
    }

    public CalendarSettingsId Id { get; } = null!;

    public string TimeZone { get; private set; } = default!;

    public HolidayCreationStrategy HolidayCreationStrategy { get; private set; }

    public static CalendarSettings Create(
        CalendarSettingsId id,
        string timeZone,
        HolidayCreationStrategy holidayCreationStrategy)
    {
        CalendarSettings calendarSettings = new(id, timeZone, holidayCreationStrategy);

        calendarSettings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(calendarSettings));

        return calendarSettings;
    }
}
