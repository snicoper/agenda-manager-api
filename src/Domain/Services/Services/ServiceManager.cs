using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Common.ValueObjects.Duration;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Services;

public class ServiceManager(
    IServiceRepository serviceRepository,
    ICalendarRepository calendarRepository)
{
    public async Task<Result<Service>> CreateServiceAsync(
        ServiceId serviceId,
        CalendarId calendarId,
        Duration duration,
        string name,
        string description,
        ColorScheme colorScheme,
        bool isActive = true,
        CancellationToken cancellationToken = default)
    {
        if (await CalendarIdExistsAsync(calendarId, cancellationToken))
        {
            return CalendarErrors.CalendarNotFound.ToResult<Service>();
        }

        if (await NameExistsAsync(name, cancellationToken))
        {
            return ServiceErrors.NameAlreadyExists;
        }

        var service = Service.Create(serviceId, calendarId, duration, name, description, colorScheme, isActive);

        return Result.Create(service);
    }

    public async Task<Result<Service>> UpdateServiceAsync(
        ServiceId serviceId,
        Duration duration,
        string name,
        string description,
        ColorScheme colorScheme,
        CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(serviceId, cancellationToken);

        if (service is null)
        {
            return ServiceErrors.NotFound;
        }

        if (await NameExistsAsync(name, cancellationToken))
        {
            return ServiceErrors.NameAlreadyExists;
        }

        service.Update(duration, name, description, colorScheme);

        return Result.Create(service);
    }

    public async Task<Result> DeleteServiceAsync(ServiceId serviceId, CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(serviceId, cancellationToken);

        if (service is null)
        {
            return ServiceErrors.NotFound;
        }

        return Result.Success();
    }

    private async Task<bool> CalendarIdExistsAsync(CalendarId calendarId, CancellationToken cancellationToken)
    {
        var exists = await calendarRepository.CalendarIdExistsAsync(calendarId, cancellationToken);

        return exists;
    }

    private async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await serviceRepository.NameExistsAsync(name, cancellationToken);

        return exists;
    }
}
