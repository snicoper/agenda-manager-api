using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.Errors;
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
    IAppointmentRepository appointmentRepository,
    ICalendarRepository calendarRepository,
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
        // 1. Get calendar and check if exists.
        var calendar = await calendarRepository.GetByIdWithSettingsAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // 2. Determine initial state based on creation strategy.
        var statusResult = confirmationStrategyPolicy.DetermineInitialStatus(calendar);

        // 3. Validate calendar and holidays.
        var holidayResult = await holidayAvailabilityPolicy.IsAvailableAsync(calendar.Id, period, cancellationToken);

        if (holidayResult.IsFailure)
        {
            return holidayResult.MapToValue<Appointment>();
        }

        // 4. Validate appointment overlapping if required.
        var overlapResult = await overlapPolicy.IsOverlappingAsync(
            calendar: calendar,
            period: period,
            cancellationToken: cancellationToken);

        if (overlapResult.IsFailure)
        {
            return overlapResult.MapToValue<Appointment>();
        }

        // 5. Validate resource availability if required.
        var resourceResult = await resourceAvailabilityPolicy.IsAvailableAsync(
            calendar: calendar,
            resources: resources,
            period: period,
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
            calendarId: calendar.Id,
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
        // 1. Get appointment and check if exists.
        var appointment = await appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);

        if (appointment is null)
        {
            return AppointmentErrors.AppointmentNotFound;
        }

        // 2. Get calendar and check if exists.
        var calendar = await calendarRepository.GetByIdWithSettingsAsync(appointment.CalendarId, cancellationToken);

        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // 3. Validate state is valid for update.
        if (appointment.CurrentState.Value is not
            (AppointmentStatus.Pending or AppointmentStatus.Accepted or AppointmentStatus.RequiresRescheduling))
        {
            return AppointmentErrors.AppointmentStatusInvalidForUpdate;
        }

        // 4. Validate calendar and holidays.
        var holidayResult = await holidayAvailabilityPolicy.IsAvailableAsync(
            calendarId: calendar.Id,
            period: period,
            cancellationToken: cancellationToken);

        if (holidayResult.IsFailure)
        {
            return holidayResult.MapToValue<Appointment>();
        }

        // 5. Validate appointment overlapping if required.
        var overlapResult = await overlapPolicy.IsOverlappingAsync(
            calendar: calendar,
            period: period,
            cancellationToken: cancellationToken);

        if (overlapResult.IsFailure)
        {
            return overlapResult.MapToValue<Appointment>();
        }

        // 6. Validate resource availability if required.
        var resourceResult = await resourceAvailabilityPolicy.IsAvailableAsync(
            calendar: calendar,
            resources: resources,
            period: period,
            cancellationToken: cancellationToken);

        if (resourceResult.IsFailure)
        {
            return resourceResult.MapToValue<Appointment>();
        }

        // 7. Validate service requirements.
        var serviceResult = await serviceRequirementsPolicy.IsSatisfiedByAsync(
            serviceId: appointment.ServiceId,
            resources: resources,
            cancellationToken: cancellationToken);

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
