﻿using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Constants;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars.Services;

public class CalendarHolidayManager(
    ICalendarRepository calendarRepository,
    ICalendarConfigurationRepository calendarConfigurationRepository,
    IAppointmentRepository appointmentRepository)
{
    public async Task<Result<CalendarHoliday>> CreateHolidayAsync(
        CalendarId calendarId,
        Period period,
        WeekDays weekDays,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        var holidayCreateStrategy = await calendarConfigurationRepository
            .GetBySelectedKeyAsync(
                calendarId: calendarId,
                selectedKey: CalendarConfigurationKeys.HolidayCreateStrategy.Key,
                cancellationToken: cancellationToken);

        if (holidayCreateStrategy is null)
        {
            return CalendarConfigurationErrors.KeyNotFound;
        }

        var overlappingAppointments = await appointmentRepository.GetOverlappingAppointmentsAsync(
            calendarId: calendarId,
            period: period,
            cancellationToken: cancellationToken);

        if (overlappingAppointments.Count != 0)
        {
            switch (holidayCreateStrategy.SelectedKey)
            {
                case CalendarConfigurationKeys.HolidayCreateStrategy.RejectIfOverlapping:
                    return CalendarHolidayErrors.CreateOverlappingReject;
                case CalendarConfigurationKeys.HolidayCreateStrategy.CancelOverlapping:
                    await CancelOverlappingAppointmentsAsync(overlappingAppointments, cancellationToken);
                    break;
                case CalendarConfigurationKeys.HolidayCreateStrategy.AllowOverlapping:
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
            weekDays: weekDays,
            name: name,
            description: description);

        calendar.AddHoliday(holiday);
        calendarRepository.Update(calendar);

        return Result.Success(holiday);
    }

    private Task CancelOverlappingAppointmentsAsync(
        List<Appointment> overlappingAppointments,
        CancellationToken cancellationToken)
    {
        foreach (var appointment in overlappingAppointments)
        {
            // TODO: Establecer como RequiresRescheduling.
        }

        return Task.CompletedTask;
    }
}
