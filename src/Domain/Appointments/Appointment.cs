﻿using AgendaManager.Domain.Appointments.Entities;
using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.Exceptions;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments;

public sealed class Appointment : AggregateRoot
{
    private readonly List<AppointmentStatusHistory> _statusHistories = [];
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
        AppointmentCurrentState currentState,
        List<Resource> resources)
    {
        Id = appointmentId;
        CalendarId = calendarId;
        ServiceId = serviceId;
        UserId = userId;
        Period = period;
        CurrentState = currentState;
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

    public AppointmentCurrentState CurrentState { get; private set; } = null!;

    public IReadOnlyList<AppointmentStatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    public IReadOnlyList<Resource> Resources => _resources.AsReadOnly();

    public Result ChangeState(AppointmentStatus status, string? description = null)
    {
        var changeStatusResult = CurrentState.ChangeStatus(status);
        if (changeStatusResult.IsFailure)
        {
            return changeStatusResult;
        }

        CurrentState = changeStatusResult.Value!;
        UpdateStatusHistories(description);

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
        AddDomainEvent(new AppointmentUpdatedDomainEvent(Id));

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
        if (CurrentState.Value is not (
            AppointmentStatus.Pending
            or AppointmentStatus.Accepted
            or AppointmentStatus.RequiresRescheduling))
        {
            return AppointmentErrors.OnlyPendingAndAcceptedAllowed;
        }

        return resources.Count == 0 ? AppointmentErrors.NoResourcesProvided : Result.Success();
    }

    private void UpdateStatusHistories(string? description)
    {
        DeactivateCurrentStatus();
        AddNewCurrentStatus(description);
    }

    private void DeactivateCurrentStatus()
    {
        var currentStatus = _statusHistories.FirstOrDefault(s => s.IsCurrentState);
        currentStatus?.DeactivateCurrentState();
    }

    private void AddNewCurrentStatus(string? description = null)
    {
        var newStatusHistory = AppointmentStatusHistory.Create(
            appointmentStatusChangeId: AppointmentStatusChangeId.Create(),
            appointmentId: Id,
            period: Period,
            state: CurrentState,
            description: description);

        _statusHistories.Add(newStatusHistory);

        GuardAgainstMultipleCurrentStatesInStatusHistories();

        AddDomainEvent(new AppointmentStatusHistoryCreatedDomainEvent(Id, CurrentState));
    }

    private void GuardAgainstMultipleCurrentStatesInStatusHistories()
    {
        var currentStatusCount = _statusHistories.Count(s => s.IsCurrentState);
        if (currentStatusCount != 1)
        {
            throw new AppointmentDomainException("Can't have more than one current status.");
        }
    }
}
