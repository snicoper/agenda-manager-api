using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments.Services;

public sealed class AppointmentManager(
    ICalendarConfigurationRepository configurationRepository,
    IAppointmentRepository appointmentRepository,
    ICalendarHolidayAvailabilityPolicy holidayAvailabilityPolicy,
    IAppointmentConfirmationStrategyPolicy confirmationStrategyPolicy,
    IAppointmentOverlapPolicy overlapPolicy,
    IResourceAvailabilityPolicy resourceAvailabilityPolicy,
    IServiceRequirementsPolicy serviceRequirementsPolicy)
{
    public async Task<Result<Appointment>> CreateAppointmentAsync(
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        List<Resource> resources,
        CancellationToken cancellationToken)
    {
        // 1. Get calendar configurations.
        var configurations = await configurationRepository.GetConfigurationsByCalendarIdAsync(
            calendarId,
            cancellationToken);

        // 2. Determine initial state based on creation strategy.
        var statusResult = confirmationStrategyPolicy.DetermineInitialStatus(configurations);

        // 3. Validate calendar and holidays.
        var holidayResult = await holidayAvailabilityPolicy.IsAvailableAsync(calendarId, period, cancellationToken);

        if (holidayResult.IsFailure)
        {
            return holidayResult.MapToValue<Appointment>();
        }

        // 4. Validate appointment overlapping if required.
        var overlapResult = await overlapPolicy.IsOverlappingAsync(
            calendarId: calendarId,
            period: period,
            configurations: configurations,
            cancellationToken: cancellationToken);

        if (overlapResult.IsFailure)
        {
            return overlapResult.MapToValue<Appointment>();
        }

        // 5. Validate resource availability if required.
        var resourceResult = await resourceAvailabilityPolicy.IsAvailableAsync(
            calendarId: calendarId,
            resources: resources,
            period: period,
            configurations: configurations,
            cancellationToken: cancellationToken);

        if (resourceResult.IsFailure)
        {
            return resourceResult.MapToValue<Appointment>();
        }

        // 6. Validate service requirements.
        var serviceResult = await serviceRequirementsPolicy.IsSatisfiedByAsync(serviceId, resources, cancellationToken);

        if (serviceResult.IsFailure)
        {
            return serviceResult.MapToValue<Appointment>();
        }

        // 7. Create appointment.
        var appointment = Appointment.Create(
            id: AppointmentId.Create(),
            calendarId: calendarId,
            serviceId: serviceId,
            userId: userId,
            period: period,
            status: statusResult.Value,
            resources: resources);

        // 8. Add appointment to repository.
        await appointmentRepository.AddAsync(appointment, cancellationToken);

        return appointment;
    }

    public async Task<Result<Appointment>> UpdateAppointmentAsync(
        AppointmentId appointmentId,
        Period period,
        List<Resource> resources,
        CancellationToken cancellationToken)
    {
        // 1. Get appointment.
        var appointment = await appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);

        if (appointment is null)
        {
            return AppointmentErrors.AppointmentNotFound;
        }

        // 2. Get calendar configurations.
        var configurations = await configurationRepository.GetConfigurationsByCalendarIdAsync(
            appointment.CalendarId,
            cancellationToken);

        // 3. Validate state is valid for update.
        if (appointment.CurrentState.Value is not
            (AppointmentStatus.Pending or AppointmentStatus.Accepted or AppointmentStatus.RequiresRescheduling))
        {
            return AppointmentErrors.AppointmentStatusInvalidForUpdate;
        }

        // 4. Validate calendar and holidays.
        var holidayResult = await holidayAvailabilityPolicy.IsAvailableAsync(
            appointment.CalendarId,
            period,
            cancellationToken);

        if (holidayResult.IsFailure)
        {
            return holidayResult.MapToValue<Appointment>();
        }

        // 5. Validate appointment overlapping if required.
        var overlapResult = await overlapPolicy.IsOverlappingAsync(
            calendarId: appointment.CalendarId,
            period: period,
            configurations: configurations,
            cancellationToken: cancellationToken);

        if (overlapResult.IsFailure)
        {
            return overlapResult.MapToValue<Appointment>();
        }

        // 6. Validate resource availability if required.
        var resourceResult = await resourceAvailabilityPolicy.IsAvailableAsync(
            calendarId: appointment.CalendarId,
            resources: resources,
            period: period,
            configurations: configurations,
            cancellationToken: cancellationToken);

        if (resourceResult.IsFailure)
        {
            return resourceResult.MapToValue<Appointment>();
        }

        // 7. Validate service requirements.
        var serviceResult = await serviceRequirementsPolicy.IsSatisfiedByAsync(
            appointment.ServiceId,
            resources,
            cancellationToken);

        if (serviceResult.IsFailure)
        {
            return serviceResult.MapToValue<Appointment>();
        }

        // 8. Update appointment.
        var updateResult = appointment.Update(period, resources);

        if (updateResult.IsFailure)
        {
            return updateResult.MapToValue<Appointment>();
        }

        // 9. Update appointment in repository.
        appointmentRepository.Update(appointment);

        return Result.Success(appointment);
    }

    public async Task<Result> DeleteAppointmentAsync(AppointmentId appointmentId, CancellationToken cancellationToken)
    {
        // 1. Get appointment.
        var appointment = await appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);

        if (appointment is null)
        {
            return AppointmentErrors.AppointmentNotFound;
        }

        // 2. Validate state is valid for delete.
        if (appointment.CurrentState.Value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted
            or AppointmentStatus.RequiresRescheduling))
        {
            return AppointmentErrors.AppointmentStatusInvalidForDelete;
        }

        // 3. Delete appointment from repository.
        appointmentRepository.Delete(appointment);

        // 4. Raise domain event.
        appointment.AddDomainEvent(new AppointmentDeletedDomainEvent(appointmentId));

        return Result.Success();
    }
}
