using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarService(ICalendarRepository calendarRepository)
{
    public async Task<Result<Calendar>> CreateAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var createdValidationResult = await ValidateAsync(calendar, cancellationToken);
        if (createdValidationResult.IsFailure)
        {
            return createdValidationResult.MapToValue<Calendar>();
        }

        var newCalendar = Calendar.Create(calendar.Id, calendar.Name, calendar.Description);
        await calendarRepository.AddAsync(newCalendar, cancellationToken);

        return Result.Create(newCalendar);
    }

    public async Task<Result<Calendar>> UpdateAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var updatedValidationResult = await ValidateAsync(calendar, cancellationToken);

        if (updatedValidationResult.IsFailure)
        {
            updatedValidationResult.MapToValue<Calendar>();
        }

        calendar.Update(calendar.Name, calendar.Description);
        calendarRepository.Update(calendar);

        return Result.Create(calendar);
    }

    public async Task<Result> ValidateAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(calendar.Name) || calendar.Name.Length > 50)
        {
            return CalendarErrors.InvalidFormatName;
        }

        if (string.IsNullOrWhiteSpace(calendar.Description) || calendar.Description.Length > 500)
        {
            return CalendarErrors.InvalidFormatDescription;
        }

        if (!await NameIsUniqueAsync(calendar, cancellationToken))
        {
            return CalendarErrors.NameAlreadyExists(calendar.Name);
        }

        if (!await DescriptionIsUniqueAsync(calendar, cancellationToken))
        {
            return CalendarErrors.DescriptionAlreadyExists(calendar.Description);
        }

        return Result.Success();
    }

    public async Task<bool> NameIsUniqueAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        return await calendarRepository.NameIsUniqueAsync(calendar, cancellationToken);
    }

    public async Task<bool> DescriptionIsUniqueAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        return await calendarRepository.DescriptionIsUniqueAsync(calendar, cancellationToken);
    }
}
