using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.IanaTimeZone;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarManager(
    ICalendarRepository calendarRepository,
    ICalendarNameValidationPolicy calendarNameValidationPolicy,
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
        var calendar = Calendar.Create(calendarId, name, description);
        var validationResult = await ValidateCalendarAsync(calendar, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Calendar>();
        }

        var calendarResult = AddDefaultConfigurationsAsync(
            calendar: calendar,
            calendarId: calendarId,
            ianaTimeZone: ianaTimeZone);

        if (calendarResult.IsFailure)
        {
            return calendarResult;
        }

        await calendarRepository.AddAsync(calendarResult.Value!, cancellationToken);

        return Result.Create(calendar);
    }

    public async Task<Result<Calendar>> UpdateCalendarAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateCalendarAsync(calendar, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Calendar>();
        }

        calendar.Update(calendar.Name, calendar.Description);
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

    private static Result<Calendar> AddDefaultConfigurationsAsync(
        Calendar calendar,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone)
    {
        foreach (var config in CalendarConfigurationKeys.Metadata.Options.Values.Where(o => !o.IsUnitValue))
        {
            calendar.AddConfiguration(
                CalendarConfiguration.Create(
                    id: CalendarConfigurationId.Create(),
                    calendarId: calendarId,
                    category: config.Category,
                    selectedKey: config.DefaultKey!));
        }

        calendar.AddConfiguration(
            CalendarConfiguration.Create(
                id: CalendarConfigurationId.Create(),
                calendarId: calendarId,
                category: CalendarConfigurationKeys.TimeZone.Category,
                selectedKey: ianaTimeZone.Value));

        return Result.Success(calendar);
    }

    private async Task<Result> ValidateCalendarAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        if (await calendarNameValidationPolicy.ExistsAsync(calendar.Id, calendar.Name, cancellationToken))
        {
            return CalendarErrors.NameAlreadyExists;
        }

        return Result.Success();
    }
}
