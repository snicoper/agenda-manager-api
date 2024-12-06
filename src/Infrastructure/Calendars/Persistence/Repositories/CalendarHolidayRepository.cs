﻿using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Calendars.Persistence.Repositories;

public class CalendarHolidayRepository(AppDbContext context) : ICalendarHolidayRepository
{
    public async Task<Result> IsOverlappingInPeriodByCalendarIdAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default)
    {
        var holidays = await context.CalendarHolidays
            .AnyAsync(
                ch => ch.CalendarId == calendarId
                      && ch.Period.Start <= period.End
                      && ch.Period.End >= period.Start
                      && (ch.AvailableDays & (WeekDays)(1 << (int)period.Start.DayOfWeek)) != 0,
                cancellationToken);

        return holidays ? CalendarHolidayErrors.HolidaysOverlap : Result.Success();
    }
}
