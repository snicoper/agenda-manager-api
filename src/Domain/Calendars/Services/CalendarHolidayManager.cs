using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars.Services;

public sealed class CalendarHolidayManager(
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
        var holidayConflictStrategy = await calendarConfigurationRepository
            .GetBySelectedKeyAsync(
                calendarId: calendarId,
                selectedKey: CalendarConfigurationKeys.Holidays.ConflictStrategy,
                cancellationToken: cancellationToken);

        if (holidayConflictStrategy is null)
        {
            return CalendarConfigurationErrors.KeyNotFound;
        }

        var overlappingAppointments = await appointmentRepository.GetOverlappingAppointmentsAsync(
            calendarId: calendarId,
            period: period,
            cancellationToken: cancellationToken);

        if (overlappingAppointments.Count != 0)
        {
            switch (holidayConflictStrategy.SelectedKey)
            {
                case CalendarConfigurationKeys.Holidays.ConflictOptions.RejectIfOverlapping:
                    return CalendarHolidayErrors.CreateOverlappingReject;
                case CalendarConfigurationKeys.Holidays.ConflictOptions.CancelOverlapping:
                    CancelOverlappingAppointments(overlappingAppointments);
                    break;
                case CalendarConfigurationKeys.Holidays.ConflictOptions.AllowOverlapping:
                    // Continuar con la creación del holiday.
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"{holidayConflictStrategy.SelectedKey} is not a valid value.");
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

    private void CancelOverlappingAppointments(List<Appointment> overlappingAppointments)
    {
        foreach (var appointment in overlappingAppointments)
        {
            appointment.ChangeState(AppointmentStatus.Cancelled, "Cancelled by holiday creation.");
        }

        appointmentRepository.UpdateRange(overlappingAppointments);
    }
}
