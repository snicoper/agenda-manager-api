using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.IanaTimeZone;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarManager(ICalendarRepository calendarRepository)
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
        var validationResult = await IsValidAsync(calendar, cancellationToken);

        if (validationResult.IsFailure)
        {
            validationResult.MapToValue<Calendar>();
        }

        calendar.Update(calendar.Name, calendar.Description);
        calendarRepository.Update(calendar);

        return Result.Success(calendar);
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
                CalendarConfigurationId.Create(),
                calendarId,
                CalendarConfigurationKeys.TimeZone.Category,
                ianaTimeZone.Value));

        return Result.Success(calendar);
    }

    private async Task<Result> IsValidAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        if (await NameExistsAsync(calendar, cancellationToken))
        {
            return CalendarErrors.NameAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> NameExistsAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var nameIsUnique = await calendarRepository.NameExistsAsync(calendar, cancellationToken);

        return nameIsUnique;
    }
}
