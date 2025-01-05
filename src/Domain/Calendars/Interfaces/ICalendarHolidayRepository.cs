﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarHolidayRepository
{
    Task<Result> IsOverlappingInPeriodByCalendarIdAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default);

    Task<Result> IsOverlappingInPeriodByCalendarIdExcludeSelfAsync(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        Period period,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsHolidayNameAsync(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        string name,
        CancellationToken cancellationToken = default);
}
