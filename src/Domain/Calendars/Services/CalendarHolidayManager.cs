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

namespace AgendaManager.Domain.Calendars.Services;

public sealed class CalendarHolidayManager(
    ICalendarRepository calendarRepository,
    ICalendarHolidayRepository calendarHolidayRepository,
    IAppointmentRepository appointmentRepository)
{
    public async Task<Result<CalendarHoliday>> CreateHolidayAsync(
        CalendarId calendarId,
        Period period,
        string name,
        CancellationToken cancellationToken)
    {
        // 1. Check if holiday name already exists.
        var calendarHolidayId = CalendarHolidayId.Create();
        if (await ExistsHolidayName(calendarId, calendarHolidayId, name, cancellationToken))
        {
            return CalendarHolidayErrors.NameAlreadyExists;
        }

        // 2. Get calendar and check if exists.
        var calendar = await calendarRepository.GetByIdWithSettingsAsync(calendarId, cancellationToken);
        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // 3. Get appointments overlapping.
        var overlappingAppointments = await appointmentRepository.GetOverlappingAppointmentsAsync(
            calendarId: calendarId,
            period: period,
            cancellationToken: cancellationToken);

        // 4. Check if there are overlapping appointments and use strategy.
        if (overlappingAppointments.Count != 0)
        {
            switch (calendar.Settings.HolidayConflict)
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

        // 5. Create holiday and add to calendar.
        var holiday = CalendarHoliday.Create(
            calendarHolidayId: calendarHolidayId,
            calendarId: calendar.Id,
            period: period,
            name: name);

        calendar.AddHoliday(holiday);
        calendarRepository.Update(calendar);

        // 6. Return holiday.
        return Result.Success(holiday);
    }

    private async Task<bool> ExistsHolidayName(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await calendarHolidayRepository.ExistsHolidayNameAsync(
            calendarId: calendarId,
            calendarHolidayId: calendarHolidayId,
            name: name,
            cancellationToken: cancellationToken);

        return exists;
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
