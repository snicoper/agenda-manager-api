using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Errors;

public static class AppointmentStatusChangeErrors
{
    public static Error OnlyPendingAndAcceptedAllowed => Error.Conflict(
        code: "AppointmentStatusChangeErrors.OnlyPendingAndAcceptedAllowed",
        description: "Only pending and accepted appointments can be changed.");

    public static Error OnlyPendingAllowed => Error.Conflict(
        code: "AppointmentStatusChangeErrors.OnlyPendingAllowed",
        description: "Only pending appointments can be changed.");

    public static Error OnlyPendingAndReschedulingAllowed => Error.Conflict(
        code: "AppointmentStatusChangeErrors.OnlyPendingAndReschedulingAllowed",
        description: "Only pending and rescheduling appointments can be changed.");

    public static Error AlreadyCancelledOrCompleted => Error.Conflict(
        code: "AppointmentStatusChangeErrors.AlreadyCancelledOrCompleted",
        description: "The appointment is already cancelled or completed.");

    public static Error OnlyWaitingAllowed => Error.Conflict(
        code: "AppointmentStatusChangeErrors.OnlyWaitingAllowed",
        description: "Only waiting appointments can be changed.");

    public static Error OnlyInProgressAllowed => Error.Conflict(
        code: "AppointmentStatusChangeErrors.OnlyInProgressAllowed",
        description: "Only in progress appointments can be changed.");
}
