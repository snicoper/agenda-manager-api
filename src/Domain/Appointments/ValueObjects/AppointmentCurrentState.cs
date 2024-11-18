using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.ValueObjects;

public sealed record AppointmentCurrentState
{
    private AppointmentCurrentState(AppointmentStatus value)
    {
        Value = value;
    }

    public AppointmentStatus Value { get; }

    public static Result<AppointmentCurrentState> From(AppointmentStatus value)
    {
        return new AppointmentCurrentState(value);
    }

    public static Result<AppointmentCurrentState> Create(AppointmentStatus value)
    {
        return value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted)
            ? AppointmentStatusChangeErrors.OnlyPendingAndAcceptedAllowed
            : new AppointmentCurrentState(value);
    }

    internal Result<AppointmentCurrentState> ToPending()
    {
        return Value is not AppointmentStatus.RequiresRescheduling
            ? AppointmentStatusChangeErrors.OnlyPendingAllowed
            : From(AppointmentStatus.Pending);
    }

    internal Result<AppointmentCurrentState> ToAccepted()
    {
        return Value is not (AppointmentStatus.Pending or AppointmentStatus.RequiresRescheduling)
            ? AppointmentStatusChangeErrors.OnlyPendingAndReschedulingAllowed
            : From(AppointmentStatus.Accepted);
    }

    internal Result<AppointmentCurrentState> ToWaiting()
    {
        return Value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted)
            ? AppointmentStatusChangeErrors.OnlyPendingAndAcceptedAllowed
            : From(AppointmentStatus.Waiting);
    }

    internal Result<AppointmentCurrentState> ToCancelled()
    {
        return Value is (AppointmentStatus.Cancelled or AppointmentStatus.Completed)
            ? AppointmentStatusChangeErrors.AlreadyCancelledOrCompleted
            : From(AppointmentStatus.Cancelled);
    }

    internal Result<AppointmentCurrentState> ToRequiresRescheduling()
    {
        return Value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted)
            ? AppointmentStatusChangeErrors.OnlyPendingAndAcceptedAllowed
            : From(AppointmentStatus.RequiresRescheduling);
    }

    internal Result<AppointmentCurrentState> ToInProgress()
    {
        return Value is not AppointmentStatus.Waiting
            ? AppointmentStatusChangeErrors.OnlyWaitingAllowed
            : From(AppointmentStatus.InProgress);
    }

    internal Result<AppointmentCurrentState> ToCompleted()
    {
        return Value is not AppointmentStatus.InProgress
            ? AppointmentStatusChangeErrors.OnlyInProgressAllowed
            : From(AppointmentStatus.Completed);
    }
}
