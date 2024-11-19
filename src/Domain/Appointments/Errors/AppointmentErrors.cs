using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Errors;

public static class AppointmentErrors
{
    public static Error OnlyPendingAndAcceptedAllowed =>
        Error.Conflict("Invalid status, only Pending and Accepted are allowed.");

    public static Error NoResourcesProvided => Error.Validation(
        nameof(Appointment.Resources),
        "No resources provided for this appointment.");

    public static Error MissingCreationStrategy => Error.Conflict("Creation strategy not found.");

    public static Error AppointmentNotFound => Error.NotFound("Appointment not found.");

    public static Error AppointmentStatusInvalidForUpdate => Error.Conflict("Invalid status for update.");

    public static Error AppointmentsOverlapping => Error.Conflict("Appointments overlapping.");

    public static Error AppointmentStatusInvalidForDelete => Error.Conflict("Invalid status for delete.");
}
