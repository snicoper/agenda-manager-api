﻿using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarRepository
{
    Task<Calendar?> GetByIdAsync(CalendarId id, CancellationToken cancellationToken = default);

    Task AddAsync(Calendar calendar, CancellationToken cancellationToken = default);

    void Update(Calendar calendar);

    Task<bool> NameExistsAsync(Calendar calendar, CancellationToken cancellationToken = default);

    Task<bool> DescriptionExistsAsync(Calendar calendar, CancellationToken cancellationToken = default);
}
