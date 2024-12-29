using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(ServiceId serviceId, CancellationToken cancellationToken = default);

    Task<Service?> GetByIdWithResourceTypesAsync(ServiceId serviceId, CancellationToken cancellationToken = default);

    Task<bool> AnyByResourceTypeIdAsync(ResourceTypeId resourceTypeId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<bool> HasServicesInCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    Task AddAsync(Service service, CancellationToken cancellationToken = default);

    void Delete(Service service);
}
