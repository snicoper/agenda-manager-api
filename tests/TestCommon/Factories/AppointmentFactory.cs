using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.TestCommon.Factories;

public static class AppointmentFactory
{
    public static Appointment Create(AppointmentId? appointmentId = null, Period? period = null)
    {
        var appointment = Appointment.Create(
            id: appointmentId ?? AppointmentId.Create(),
            period: period ?? PeriodFactory.Create());

        return appointment;
    }
}
