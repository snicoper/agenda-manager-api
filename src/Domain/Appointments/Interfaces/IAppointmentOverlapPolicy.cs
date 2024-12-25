using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Appointments.Interfaces;

public interface IAppointmentOverlapPolicy
{
    Task<Result> IsOverlappingAsync(
        Calendar calendar,
        Period period,
        CancellationToken cancellationToken = default);
}
