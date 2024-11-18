using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Appointments.Interfaces;

public interface IAppointmentOverlapPolicy
{
    Task<Result> IsOverlappingAsync(
        CalendarId calendarId,
        Period period,
        List<CalendarConfiguration> configurations,
        CancellationToken cancellationToken = default);
}
