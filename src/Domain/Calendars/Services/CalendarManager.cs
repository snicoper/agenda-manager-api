﻿using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarManager(ICalendarRepository calendarRepository)
{
    public async Task<Result<Calendar>> CreateCalendarAsync(
        CalendarId calendarId,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        Calendar calendar = new(calendarId, name, description);
        var validationResult = await IsValidAsync(calendar, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Calendar>();
        }

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

    public async Task<Result> IsValidAsync(Calendar calendar, CancellationToken cancellationToken)
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

    public async Task<bool> NameExistsAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var nameIsUnique = await calendarRepository.NameExistsAsync(calendar, cancellationToken);

        return nameIsUnique;
    }

    public async Task<bool> DescriptionExistsAsync(Calendar calendar, CancellationToken cancellationToken)
    {
        var descriptionIsUnique = await calendarRepository.DescriptionExistsAsync(calendar, cancellationToken);

        return descriptionIsUnique;
    }
}
