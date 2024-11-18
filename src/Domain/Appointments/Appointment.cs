﻿using AgendaManager.Domain.Appointments.Entities;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Events;
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

    public Result ChangeState(AppointmentStatus status, string? description)
    {
        var changeStatusResult = status switch
        {
            AppointmentStatus.Pending => State.ToPending(),
            AppointmentStatus.Accepted => State.ToAccepted(),
            AppointmentStatus.Cancelled => State.ToCancelled(),
            AppointmentStatus.RequiresRescheduling => State.ToRequiresRescheduling(),
            AppointmentStatus.Waiting => State.ToWaiting(),
            AppointmentStatus.InProgress => State.ToInProgress(),
            AppointmentStatus.Completed => State.ToCompleted(),
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

        if (changeStatusResult.IsFailure)
        {
            return changeStatusResult;
        }

        State = changeStatusResult.Value!;

        UpdateStatusChanges(description);

        AddDomainEvent(new AppointmentStatusChangedDomainEvent(Id, changeStatusResult.Value!));

        return changeStatusResult;
    }

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

        var currentState = AppointmentCurrentState.From(status);

        if (currentState.IsFailure)
        {
            return currentState.MapToValue<Appointment>();
        }

        Appointment appointment = new(id, calendarId, serviceId, userId, period, currentState.Value!, resources);

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

        Period = period;

        _resources.Clear();
        _resources.AddRange(resources);

        AddDomainEvent(new AppointmentUpdatedDomainEvent(Id, period, resources));

        return Result.Success();
    }

    private bool AreResourceListEqual(List<Resource> other)
    {
        if (_resources.Count != other.Count)
        {
            return false;
        }

        var equals = _resources.Select(r => r.Id)
            .OrderBy(id => id)
            .SequenceEqual(other.Select(o => o.Id).OrderBy(id => id));

        return equals;
    }

    private Result ValidateForUpdate(List<Resource> resources)
    {
        if (State.Value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted))
        {
            return AppointmentErrors.OnlyPendingAndAcceptedAllowed;
        }

        if (resources.Count == 0)
        {
            return AppointmentErrors.NoResourcesProvided;
        }

        if (_resources.Count == 0)
        {
            return AppointmentErrors.NoResourcesFound;
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

    private void AddNewCurrentStatus(string? description)
    {
        var newStatusChange = AppointmentStatusChange.Create(
            appointmentStatusChangeId: AppointmentStatusChangeId.Create(),
            appointmentId: Id,
            period: Period,
            state: State,
            isCurrentStatus: true,
            description: description);

        _statusChanges.Add(newStatusChange);
    }
}
