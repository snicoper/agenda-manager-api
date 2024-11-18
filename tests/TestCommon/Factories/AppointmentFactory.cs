using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class AppointmentFactory
{
    public static Result<Appointment> CreateAppointment(
        AppointmentId? appointmentId = null,
        CalendarId? calendarId = null,
        ServiceId? serviceId = null,
        UserId? userId = null,
        Period? period = null,
        AppointmentStatus? status = null,
        List<Resource>? resources = null)
    {
        resources ??=
        [
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource()
        ];

        var appointment = Appointment.Create(
            id: appointmentId ?? AppointmentId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            serviceId: serviceId ?? ServiceId.Create(),
            userId: userId ?? UserId.Create(),
            period: period ?? PeriodFactory.Create(),
            status: status ?? AppointmentStatus.Pending,
            resources: resources);

        return appointment;
    }

    /// <summary>
    /// Creates an appointment with any status, bypassing transition rules for testing.
    /// </summary>
    public static Result<Appointment> CreateAppointmentForTesting(
        AppointmentId? appointmentId = null,
        CalendarId? calendarId = null,
        ServiceId? serviceId = null,
        UserId? userId = null,
        Period? period = null,
        AppointmentStatus? status = null,
        List<Resource>? resources = null)
    {
        resources ??=
        [
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource(),
            ResourceFactory.CreateResource()
        ];

        var appointment = Appointment.CreateForTesting(
            id: appointmentId ?? AppointmentId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            serviceId: serviceId ?? ServiceId.Create(),
            userId: userId ?? UserId.Create(),
            period: period ?? PeriodFactory.Create(),
            status: status ?? AppointmentStatus.Pending,
            resources: resources);

        return appointment;
    }
}
