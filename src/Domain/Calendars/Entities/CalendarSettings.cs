using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Utils;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarSettings : AuditableEntity
{
    private CalendarSettings()
    {
    }

    private CalendarSettings(
        CalendarSettingsId id,
        CalendarId calendarId,
        string timeZone,
        HolidayCreationStrategy holidayCreationStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayCreationStrategy);

        GuardAgainstInvalidTimeZone(timeZone);

        Id = id;
        CalendarId = calendarId;
        TimeZone = timeZone;
        HolidayCreationStrategy = holidayCreationStrategy;
    }

    public CalendarSettingsId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public string TimeZone { get; private set; } = default!;

    public HolidayCreationStrategy HolidayCreationStrategy { get; private set; }

    internal static CalendarSettings Create(
        CalendarSettingsId id,
        CalendarId calendarId,
        string timeZone,
        HolidayCreationStrategy holidayCreationStrategy)
    {
        CalendarSettings calendarSettings = new(id, calendarId, timeZone, holidayCreationStrategy);

        calendarSettings.AddDomainEvent(new CalendarSettingsCreatedDomainEvent(calendarSettings));

        return calendarSettings;
    }

    internal void Update(string timeZone, HolidayCreationStrategy holidayCreationStrategy)
    {
        ArgumentNullException.ThrowIfNull(holidayCreationStrategy);

        GuardAgainstInvalidTimeZone(timeZone);

        if (TimeZone == timeZone && HolidayCreationStrategy == holidayCreationStrategy)
        {
            return;
        }

        TimeZone = timeZone;
        HolidayCreationStrategy = holidayCreationStrategy;
    }

    private static void GuardAgainstInvalidTimeZone(string timeZone)
    {
        ArgumentNullException.ThrowIfNull(timeZone);

        var timeZoneInfoResult = TimeZoneUtils.GetTimeZoneInfoFromIana(timeZone);

        if (timeZoneInfoResult.IsFailure)
        {
            throw new CalendarSettingsException(timeZoneInfoResult.Error?.FirstError()?.Description!);
        }
    }
}
