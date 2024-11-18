using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Appointments.Policies;

public class AppointmentOverlapPolicy : IAppointmentOverlapPolicy
{
    public Task<Result> IsOverlappingAsync(
        CalendarId calendarId,
        Period period,
        List<CalendarConfiguration> configurations,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
