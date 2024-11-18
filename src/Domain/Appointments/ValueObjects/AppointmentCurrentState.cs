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

    internal Result<AppointmentCurrentState> ChangeState(AppointmentStatus status)
    {
        var changeStatusResult = status switch
        {
            AppointmentStatus.Pending => ToPending(),
            AppointmentStatus.Accepted => ToAccepted(),
            AppointmentStatus.Cancelled => ToCancelled(),
            AppointmentStatus.RequiresRescheduling => ToRequiresRescheduling(),
            AppointmentStatus.Waiting => ToWaiting(),
            AppointmentStatus.InProgress => ToInProgress(),
            AppointmentStatus.Completed => ToCompleted(),
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

        return changeStatusResult;
    }

    private Result<AppointmentCurrentState> ToPending()
    {
        return Value is not AppointmentStatus.RequiresRescheduling
            ? AppointmentStatusChangeErrors.OnlyPendingAllowed
            : From(AppointmentStatus.Pending);
    }

    private Result<AppointmentCurrentState> ToAccepted()
    {
        return Value is not (AppointmentStatus.Pending or AppointmentStatus.RequiresRescheduling)
            ? AppointmentStatusChangeErrors.OnlyPendingAndReschedulingAllowed
            : From(AppointmentStatus.Accepted);
    }

    private Result<AppointmentCurrentState> ToWaiting()
    {
        return Value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted)
            ? AppointmentStatusChangeErrors.OnlyPendingAndAcceptedAllowed
            : From(AppointmentStatus.Waiting);
    }

    private Result<AppointmentCurrentState> ToCancelled()
    {
        return Value is (AppointmentStatus.Cancelled or AppointmentStatus.Completed)
            ? AppointmentStatusChangeErrors.AlreadyCancelledOrCompleted
            : From(AppointmentStatus.Cancelled);
    }

    private Result<AppointmentCurrentState> ToRequiresRescheduling()
    {
        return Value is not (AppointmentStatus.Pending or AppointmentStatus.Accepted)
            ? AppointmentStatusChangeErrors.OnlyPendingAndAcceptedAllowed
            : From(AppointmentStatus.RequiresRescheduling);
    }

    private Result<AppointmentCurrentState> ToInProgress()
    {
        return Value is not AppointmentStatus.Waiting
            ? AppointmentStatusChangeErrors.OnlyWaitingAllowed
            : From(AppointmentStatus.InProgress);
    }

    private Result<AppointmentCurrentState> ToCompleted()
    {
        return Value is not AppointmentStatus.InProgress
            ? AppointmentStatusChangeErrors.OnlyInProgressAllowed
            : From(AppointmentStatus.Completed);
    }
}
