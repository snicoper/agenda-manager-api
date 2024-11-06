﻿using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class CalendarFactory
{
    public static Calendar CreateCalendar(
        CalendarId? calendarId = null,
        string? name = null,
        string? description = null)
    {
        var calendar = Calendar.Create(
            calendarId ?? CalendarId.Create(),
            name ?? "My calendar",
            description ?? "Description of my calendar");

        return calendar;
    }
}
