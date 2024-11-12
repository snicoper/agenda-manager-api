using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarManager(
    ICalendarRepository calendarRepository,
    ICalendarSettingsRepository calendarSettingsRepository,
    IAppointmentRepository appointmentRepository)
{
    public async Task<Result<Calendar>> CreateCalendarAsync(
        CalendarId calendarId,
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

    private async Task<Result<CalendarHoliday>> CreateHolidayAsync(
        CalendarId calendarId,
        Period period,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        var settings = await calendarSettingsRepository.GetSettingsByCalendarIdAsync(calendarId, cancellationToken);

        if (settings is null)
        {
            return CalendarSettingsErrors.CalendarSettingsNotFound;
        }

        var overlappingAppointments = await appointmentRepository
            .GetOverlappingAppointmentsAsync(calendarId, period, cancellationToken);

        if (overlappingAppointments.Count != 0)
        {
            switch (settings.HolidayCreationStrategy)
            {
                case HolidayCreationStrategy.RejectIfOverlapping:
                    return CalendarHolidayErrors.CreateOverlappingReject;
                case HolidayCreationStrategy.CancelOverlapping:
                    // TODO: Implement this strategy
                    // Marcar los appointments como cancelados.
                    break;
                case HolidayCreationStrategy.AllowOverlapping:
                    // Continuar con la creación del holiday.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var calendar = await calendarRepository.GetByIdAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        var holiday = CalendarHoliday.Create(
            calendarHolidayId: CalendarHolidayId.Create(),
            calendarId: calendar.Id,
            period: period,
            weekDays: WeekDays.All,
            name: name,
            description: description);

        calendar.AddHoliday(holiday);
        calendarRepository.Update(calendar);

        return Result.Success(holiday);
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
