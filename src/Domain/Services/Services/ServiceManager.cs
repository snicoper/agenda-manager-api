using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Services;

public class ServiceManager(
    IServiceRepository serviceRepository,
    ICalendarRepository calendarRepository,
    IAppointmentRepository appointmentRepository)
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
        if (!await ExistsByCalendarIdAsync(calendarId, cancellationToken))
        {
            return CalendarErrors.CalendarNotFound.ToResult<Service>();
        }

        if (await ExistsByNameAsync(name, cancellationToken))
        {
            return ServiceErrors.NameAlreadyExists;
        }

        var service = Service.Create(serviceId, calendarId, duration, name, description, colorScheme, isActive);

        await serviceRepository.AddAsync(service, cancellationToken);

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
            return ServiceErrors.ServiceNotFound;
        }

        if (await ExistsByNameAsync(name, cancellationToken))
        {
            return ServiceErrors.NameAlreadyExists;
        }

        service.Update(duration, name, description, colorScheme);

        return Result.Success(service);
    }

    public async Task<Result> DeleteServiceAsync(ServiceId serviceId, CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(serviceId, cancellationToken);

        if (service is null)
        {
            return ServiceErrors.ServiceNotFound;
        }

        // Service deletion is not allowed when it has dependent appointments.
        var appointments = appointmentRepository.GetAllByServiceId(serviceId, cancellationToken);

        if (appointments.Count != 0)
        {
            return ServiceErrors.HasAssociatedAppointments;
        }

        serviceRepository.Delete(service);

        return Result.Success();
    }

    private async Task<bool> ExistsByCalendarIdAsync(CalendarId calendarId, CancellationToken cancellationToken)
    {
        var exists = await calendarRepository.ExistsByCalendarIdAsync(calendarId, cancellationToken);

        return exists;
    }

    private async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await serviceRepository.ExistsByNameAsync(name, cancellationToken);

        return exists;
    }
}
