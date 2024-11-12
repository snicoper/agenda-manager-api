﻿using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarFactory
{
    public static Calendar CreateCalendar(
        CalendarId? calendarId = null,
        string? name = null,
        string? description = null,
        bool? isActive = null)
    {
        calendarId ??= CalendarId.Create();

        var settings = CalendarSettingsFactory.CreateCalendarSettings(calendarId: calendarId);
        var calendar = Calendar.Create(
            id: calendarId,
            settings: settings,
            name: name ?? "My calendar",
            description: description ?? "Description of my calendar",
            active: isActive ?? true);

        return calendar;
    }
}
