using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.Aggregates;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments;

public class Appointment : AggregateRoot
{
    private readonly List<AppointmentStatusChange> _statusChanges = [];

    private Appointment()
    {
    }

    private Appointment(AppointmentId appointmentId, Period period)
    {
        Id = appointmentId;
        Period = period;
    }

    public AppointmentId Id { get; } = null!;

    public ServiceId ServiceId { get; private set; } = null!;

    public Service Service { get; private set; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public UserId UserId { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;

    public IReadOnlyCollection<AppointmentStatusChange> StatusChanges => _statusChanges.AsReadOnly();

    public static Appointment Create(AppointmentId id, Period period)
    {
        Appointment appointment = new(id, period);

        appointment.AddDomainEvent(new AppointmentCreatedDomainEvent(appointment.Id));

        return appointment;
    }
}
