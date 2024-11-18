using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Services;

public class AppointmentManager
{
    public Task<Result<Appointment>> CreateAppointment()
    {
        return Task.FromResult(Result.Success<Appointment>());
    }

    public Task<Result<Appointment>> UpdateAppointment()
    {
        return Task.FromResult(Result.Success<Appointment>());
    }
}
