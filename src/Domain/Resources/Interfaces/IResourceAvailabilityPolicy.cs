using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Resources.Interfaces;

public interface IResourceAvailabilityPolicy
{
    Task<Result> IsAvailableAsync(
        Calendar calendar,
        List<Resource> resources,
        Period period,
        CancellationToken cancellationToken = default);
}
