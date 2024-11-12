using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarSettingsFactory
{
    public static CalendarSettings CreateCalendarSettings(
        CalendarSettingsId? calendarSettingsId = null,
        CalendarId? calendarId = null,
        IanaTimeZone? timeZone = null,
        HolidayCreationStrategy? holidayCreationStrategy = null)
    {
        var settings = CalendarSettings.Create(
            id: calendarSettingsId ?? CalendarSettingsId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            ianaTimeZone: timeZone ?? IanaTimeZone.FromIana("Europe/Madrid"),
            holidayCreationStrategy: holidayCreationStrategy ?? HolidayCreationStrategy.CancelOverlapping);

        return settings;
    }
}
