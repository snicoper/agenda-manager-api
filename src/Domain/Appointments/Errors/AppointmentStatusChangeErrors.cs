using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Errors;

public static class AppointmentStatusChangeErrors
{
    public static Error OnlyPendingAndAcceptedAllowed =>
        Error.Conflict("Only pending and accepted appointments can be changed.");

    public static Error OnlyPendingAllowed =>
        Error.Conflict("Only pending appointments can be changed.");

    public static Error OnlyPendingAndReschedulingAllowed =>
        Error.Conflict("Only pending and rescheduling appointments can be changed.");

    public static Error OnlyPendingAndAcceptedAndReschedulingAllowed =>
        Error.Conflict("Only pending, accepted and rescheduling appointments can be changed.");

    public static Error OnlyWaitingAllowed =>
        Error.Conflict("Only waiting appointments can be changed.");

    public static Error OnlyInProgressAllowed =>
        Error.Conflict("Only in progress appointments can be changed.");
}
