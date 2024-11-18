using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments.Services;

public class AppointmentManager(ICalendarConfigurationRepository calendarConfigurationRepository)
{
    public async Task<Result<Appointment>> CreateAppointmentAsync(
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        List<Resource> resources,
        CancellationToken cancellationToken)
    {
        var configurations = await calendarConfigurationRepository.GetConfigurationsByCalendarIdAsync(
            calendarId,
            cancellationToken);

        return Result.Success<Appointment>();
    }

    public Task<Result<Appointment>> UpdateAppointment()
    {
        return Task.FromResult(Result.Success<Appointment>());
    }
}
