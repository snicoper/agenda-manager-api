using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarManager(
    ICalendarRepository calendarRepository,
    IHasAppointmentsInCalendarPolicy appointmentsInCalendarPolicy,
    IHasResourcesInCalendarPolicy resourcesInCalendarPolicy,
    IHasServicesInCalendarPolicy servicesInCalendarPolicy)
{
    public async Task<Result<Calendar>> CreateCalendarAsync(
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        // Create calendar settings.
        var settings = CalendarSettings.Create(
            calendarSettingsId: CalendarSettingsId.Create(),
            calendarId: calendarId,
            timeZone: ianaTimeZone,
            appointmentConfirmationRequirement: AppointmentConfirmationRequirementStrategy.Require,
            appointmentOverlapping: AppointmentOverlappingStrategy.Reject,
            holidayConflict: HolidayConflictStrategy.Reject,
            resourceScheduleValidation: ResourceScheduleValidationStrategy.Validate);

        // Create calendar and validate.
        var calendar = Calendar.Create(calendarId, settings, name, description);
        var validationResult = await ValidateCalendarAsync(calendarId, calendar.Name, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Calendar>();
        }

        await calendarRepository.AddAsync(calendar, cancellationToken);

        return Result.Create(calendar);
    }

    public async Task<Result<Calendar>> UpdateCalendarAsync(
        CalendarId calendarId,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        var calendar = await calendarRepository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar == null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        var validationResult = await ValidateCalendarAsync(calendarId, name, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Calendar>();
        }

        calendar.Update(name, description);
        calendarRepository.Update(calendar);

        return Result.Success(calendar);
    }

    public async Task<Result> DeleteCalendarAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        // 1. Check if calendar has appointments.
        var hasAppointments = await appointmentsInCalendarPolicy.IsSatisfiedByAsync(calendar.Id, cancellationToken);

        if (hasAppointments)
        {
            return CalendarErrors.CannotDeleteCalendarWithAppointments;
        }

        // 2. Check if calendar has resources.
        var hasResources = await resourcesInCalendarPolicy.IsSatisfiedByAsync(calendar.Id, cancellationToken);

        if (hasResources)
        {
            return CalendarErrors.CannotDeleteCalendarWithResources;
        }

        // 3. Check if calendar has services.
        var hasServices = await servicesInCalendarPolicy.IsSatisfiedByAsync(calendar.Id, cancellationToken);

        if (hasServices)
        {
            return CalendarErrors.CannotDeleteCalendarWithServices;
        }

        calendarRepository.Delete(calendar);

        return Result.Success();
    }

    private async Task<Result> ValidateCalendarAsync(
        CalendarId calendarId,
        string name,
        CancellationToken cancellationToken)
    {
        if (await calendarRepository.ExistsByNameAsync(calendarId, name, cancellationToken))
        {
            return CalendarErrors.NameAlreadyExists;
        }

        return Result.Success();
    }
}
