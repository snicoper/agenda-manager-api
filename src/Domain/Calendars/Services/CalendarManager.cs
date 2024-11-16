using AgendaManager.Domain.Calendars.Constants;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarManager(
    ICalendarRepository calendarRepository,
    ICalendarConfigurationOptionRepository calendarConfigurationOptionRepository)
{
    public async Task<Result<Calendar>> CreateCalendarAsync(
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        var calendar = Calendar.Create(calendarId, name, description);
        var validationResult = await IsValidAsync(calendar, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Calendar>();
        }

        calendar = await AddDefaultConfigurationsAsync(
            calendar: calendar,
            calendarId: calendarId,
            ianaTimeZone: ianaTimeZone,
            cancellationToken: cancellationToken);

        await calendarRepository.AddAsync(calendar, cancellationToken);

        return Result.Create(calendar);
    }

    public async Task<Result<Calendar>> UpdateCalendarAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var validationResult = await IsValidAsync(calendar, cancellationToken);
        if (validationResult.IsFailure)
        {
            validationResult.MapToValue<Calendar>();
        }

        calendar.Update(calendar.Name, calendar.Description);
        calendarRepository.Update(calendar);

        return Result.Create(calendar);
    }

    private async Task<Calendar> AddDefaultConfigurationsAsync(
        Calendar calendar,
        CalendarId calendarId,
        IanaTimeZone ianaTimeZone,
        CancellationToken cancellationToken)
    {
        var options = await calendarConfigurationOptionRepository.GetAllAsync(cancellationToken);

        var configurations = options
            .Where(cco => cco.DefaultValue && cco.Key != CalendarConfigurationKeys.CustomValues.Key)
            .Select(
                cco => CalendarConfiguration.Create(
                    id: CalendarConfigurationId.Create(),
                    calendarId: calendarId,
                    category: cco.Category,
                    selectedKey: cco.Key));

        foreach (var configuration in configurations)
        {
            calendar.AddConfiguration(configuration);
        }

        var ianaTimeZoneOption = CalendarConfiguration.Create(
            id: CalendarConfigurationId.Create(),
            calendarId: calendarId,
            category: CalendarConfigurationKeys.CustomValues.IanaTimeZone,
            selectedKey: ianaTimeZone.Value);

        calendar.AddConfiguration(ianaTimeZoneOption);

        return calendar;
    }

    private async Task<Result> IsValidAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        if (await NameExistsAsync(calendar, cancellationToken))
        {
            return CalendarErrors.NameAlreadyExists;
        }

        if (await DescriptionExistsAsync(calendar, cancellationToken))
        {
            return CalendarErrors.DescriptionAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> NameExistsAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var nameIsUnique = await calendarRepository.NameExistsAsync(calendar, cancellationToken);

        return nameIsUnique;
    }

    private async Task<bool> DescriptionExistsAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var descriptionIsUnique = await calendarRepository.DescriptionExistsAsync(calendar, cancellationToken);

        return descriptionIsUnique;
    }
}
