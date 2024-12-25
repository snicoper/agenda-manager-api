using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars.Services;

public sealed class CalendarHolidayManager(
    ICalendarRepository calendarRepository,
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
        // 1. Get calendar and check if exists.
        var calendar = await calendarRepository.GetByIdWithSettingsAsync(calendarId, cancellationToken);

        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // 2. Get appointments overlapping.
        var overlappingAppointments = await appointmentRepository.GetOverlappingAppointmentsAsync(
            calendarId: calendarId,
            period: period,
            cancellationToken: cancellationToken);

        // 3. Check if there are overlapping appointments and use strategy.
        if (overlappingAppointments.Count != 0)
        {
            switch (calendar.Settings.HolidayAppointmentHandling)
            {
                case HolidayConflictStrategy.Reject:
                    return CalendarHolidayErrors.CreateOverlappingReject;
                case HolidayConflictStrategy.Cancel:
                    CancelOverlappingAppointments(overlappingAppointments);
                    break;
                case HolidayConflictStrategy.Allow:
                    // Continuar con la creación del holiday.
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(HolidayConflictStrategy)} is not a valid value.");
            }
        }

        // 4. Create holiday and add to calendar.
        var holiday = CalendarHoliday.Create(
            calendarHolidayId: CalendarHolidayId.Create(),
            calendarId: calendar.Id,
            period: period,
            weekDays: weekDays,
            name: name,
            description: description);

        calendar.AddHoliday(holiday);
        calendarRepository.Update(calendar);

        // 5. Return holiday.
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
