using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Interfaces;

public interface IServiceRequirementsPolicy
{
    Task<Result> IsSatisfiedByAsync(
        ServiceId serviceId,
        List<Resource> resources,
        CancellationToken cancellationToken = default);
}
