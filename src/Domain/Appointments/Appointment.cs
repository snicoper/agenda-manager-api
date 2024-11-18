using AgendaManager.Domain.Appointments.Entities;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.Exceptions;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments;

public sealed class Appointment : AggregateRoot
{
    private readonly List<AppointmentStatusChange> _statusChanges = [];
    private readonly List<Resource> _resources = [];

    private Appointment()
    {
    }

    private Appointment(
        AppointmentId appointmentId,
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        AppointmentCurrentState state,
        List<Resource> resources)
    {
        Id = appointmentId;
        CalendarId = calendarId;
        ServiceId = serviceId;
        UserId = userId;
        Period = period;
        State = state;
        _resources = resources;
    }

    public AppointmentId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public ServiceId ServiceId { get; private set; } = null!;

    public Service Service { get; private set; } = null!;

    public UserId UserId { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public AppointmentCurrentState State { get; private set; } = null!;

    public IReadOnlyList<AppointmentStatusChange> StatusChanges => _statusChanges.AsReadOnly();

    public IReadOnlyList<Resource> Resources => _resources.AsReadOnly();

    public Result ChangeState(AppointmentStatus status, string? description = null)
    {
        var changeStatusResult = State.ChangeState(status);
        if (changeStatusResult.IsFailure)
        {
            return changeStatusResult;
        }

        State = changeStatusResult.Value!;
        UpdateStatusChanges(description);

        return changeStatusResult;
    }

#if DEBUG
    /// <summary>
    /// Factory method for testing purposes only.
    /// Allows creation of appointments with any status, bypassing normal state transition rules.
    /// This is needed to test scenarios with invalid state transitions.
    /// </summary>
    internal static Result<Appointment> CreateForTesting(
        AppointmentId id,
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        AppointmentStatus status,
        List<Resource> resources)
    {
        var currentState = AppointmentCurrentState.From(status);

        return new Appointment(id, calendarId, serviceId, userId, period, currentState.Value!, resources);
    }
#endif

    internal static Result<Appointment> Create(
        AppointmentId id,
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        AppointmentStatus status,
        List<Resource> resources)
    {
        if (resources.Count == 0)
        {
            return AppointmentErrors.NoResourcesProvided;
        }

        var currentState = AppointmentCurrentState.Create(status);
        if (currentState.IsFailure)
        {
            return currentState.MapToValue<Appointment>();
        }

        Appointment appointment = new(id, calendarId, serviceId, userId, period, currentState.Value!, resources);

        appointment.AddNewCurrentStatus();
        appointment.AddDomainEvent(new AppointmentCreatedDomainEvent(appointment.Id));

        return Result.Create(appointment);
    }

    internal Result Update(Period period, List<Resource> resources)
    {
        var validation = ValidateForUpdate(resources);
        if (validation.IsFailure)
        {
            return validation;
        }

        if (period == Period && AreResourceListEqual(resources))
        {
            return Result.Success();
        }

        UpdatePeriodAndResources(period, resources);
        AddDomainEvent(new AppointmentUpdatedDomainEvent(Id, period, resources));

        return Result.Success();
    }

    private void UpdatePeriodAndResources(Period period, List<Resource> resources)
    {
        Period = period;
        _resources.Clear();
        _resources.AddRange(resources);
    }

    private bool AreResourceListEqual(List<Resource> other)
    {
        if (_resources.Count != other.Count)
        {
            return false;
        }

        var equals = _resources.Select(r => r.Id)
            .OrderBy(id => id.Value)
            .SequenceEqual(other.Select(o => o.Id).OrderBy(id => id.Value));

        return equals;
    }

    private Result ValidateForUpdate(List<Resource> resources)
    {
        if (State.Value is not (
            AppointmentStatus.Pending
            or AppointmentStatus.Accepted
            or AppointmentStatus.RequiresRescheduling))
        {
            return AppointmentErrors.OnlyPendingAndAcceptedAllowed;
        }

        if (resources.Count == 0)
        {
            return AppointmentErrors.NoResourcesProvided;
        }

        return Result.Success();
    }

    private void UpdateStatusChanges(string? description)
    {
        DeactivateCurrentStatus();
        AddNewCurrentStatus(description);
    }

    private void DeactivateCurrentStatus()
    {
        var currentStatus = _statusChanges.FirstOrDefault(s => s.IsCurrentStatus);
        currentStatus?.DeactivateCurrentStatus();
    }

    private void AddNewCurrentStatus(string? description = null)
    {
        var newStatusChange = AppointmentStatusChange.Create(
            appointmentStatusChangeId: AppointmentStatusChangeId.Create(),
            appointmentId: Id,
            period: Period,
            state: State,
            isCurrentStatus: true,
            description: description);

        _statusChanges.Add(newStatusChange);
        EnsureSingleCurrentStatus();

        AddDomainEvent(new AppointmentStatusChangedDomainEvent(Id, State));
    }

    private void EnsureSingleCurrentStatus()
    {
        var currentStatusCount = _statusChanges.Count(s => s.IsCurrentStatus);
        if (currentStatusCount != 1)
        {
            throw new AppointmentDomainException("Can't have more than one current status.");
        }
    }
}
