using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Services.Interfaces;

namespace AgendaManager.Domain.Services.Policies;

public class HasServicesInCalendarPolicy(IServiceRepository serviceRepository) : IHasServicesInCalendarPolicy
{
    public async Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken = default)
    {
        var hasServices = await serviceRepository.HasServicesInCalendarAsync(calendarId, cancellationToken);

        return hasServices;
    }
}
