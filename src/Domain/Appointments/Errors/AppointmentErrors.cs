using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Errors;

public static class AppointmentErrors
{
    public static Error OnlyPendingAndAcceptedAllowed => Error.Conflict(
        code: "AppointmentErrors.OnlyPendingAndAcceptedAllowed",
        description: "Invalid status, only Pending and Accepted are allowed.");

    public static Error NoResourcesProvided => Error.Validation(
        code: nameof(Appointment.Resources),
        description: "No resources provided for this appointment.");

    public static Error MissingCreationStrategy => Error.Conflict(
        code: "AppointmentErrors.MissingCreationStrategy",
        description: "Creation strategy not found.");

    public static Error AppointmentNotFound => Error.NotFound(
        code: "AppointmentErrors.AppointmentNotFound",
        description: "Appointment not found.");

    public static Error AppointmentStatusInvalidForUpdate => Error.Conflict(
        code: "AppointmentErrors.AppointmentStatusInvalidForUpdate",
        description: "Invalid status for update.");

    public static Error AppointmentsOverlapping => Error.Conflict(
        code: "AppointmentErrors.AppointmentsOverlapping",
        description: "Appointments overlapping.");

    public static Error AppointmentStatusInvalidForDelete => Error.Conflict(
        code: "AppointmentErrors.AppointmentStatusInvalidForDelete",
        description: "Invalid status for delete.");
}
