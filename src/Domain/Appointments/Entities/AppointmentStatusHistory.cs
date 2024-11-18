using AgendaManager.Domain.Appointments.Exceptions;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Appointments.Entities;

public sealed class AppointmentStatusHistory : AuditableEntity
{
    private AppointmentStatusHistory()
    {
    }

    private AppointmentStatusHistory(
        AppointmentStatusChangeId appointmentStatusChangeId,
        AppointmentId appointmentId,
        Period period,
        AppointmentCurrentState status,
        bool isCurrentStatus,
        string? description)
    {
        Id = appointmentStatusChangeId;
        AppointmentId = appointmentId;
        Period = period;
        CurrentState = status;
        IsCurrentStatus = isCurrentStatus;
        Description = description;
    }

    public AppointmentStatusChangeId Id { get; } = null!;

    public AppointmentId AppointmentId { get; private set; } = null!;

    public Appointment Appointment { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public AppointmentCurrentState CurrentState { get; private set; } = null!;

    public bool IsCurrentStatus { get; private set; } = true;

    public string? Description { get; private set; }

    internal static AppointmentStatusHistory Create(
        AppointmentStatusChangeId appointmentStatusChangeId,
        AppointmentId appointmentId,
        Period period,
        AppointmentCurrentState state,
        bool isCurrentStatus,
        string? description)
    {
        AgainstInvalidDescription(description);

        AppointmentStatusHistory appointmentStatusHistory = new(
            appointmentStatusChangeId,
            appointmentId,
            period,
            state,
            isCurrentStatus,
            description);

        return appointmentStatusHistory;
    }

    internal void DeactivateCurrentStatus()
    {
        if (!IsCurrentStatus)
        {
            return;
        }

        IsCurrentStatus = false;
    }

    private static void AgainstInvalidDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > 200)
        {
            throw new AppointmentStatusChangeDomainException(
                "Description is invalid or exceeds length of 500 characters.");
        }
    }
}
