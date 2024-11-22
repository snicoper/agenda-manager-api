﻿using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Appointments.Interfaces;

public interface IHasAppointmentsInCalendarPolicy
{
    Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken);
}
