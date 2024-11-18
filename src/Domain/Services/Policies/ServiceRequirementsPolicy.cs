using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Policies;

public class ServiceRequirementsPolicy : IServiceRequirementsPolicy
{
    public Task<Result> IsSatisfiedByAsync(
        ServiceId serviceId,
        List<Resource> resources,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
