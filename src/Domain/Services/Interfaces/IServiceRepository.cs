using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(ServiceId serviceId, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);

    void Delete(Service service);
}
