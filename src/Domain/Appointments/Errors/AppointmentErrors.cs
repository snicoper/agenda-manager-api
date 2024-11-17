﻿using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Errors;

public static class AppointmentErrors
{
    public static Error OnlyPendingAndAcceptedAllowed =>
        Error.Conflict("Invalid status, only Pending and Accepted are allowed.");

    public static Error NoResourcesFound => Error.Unexpected("No resources found for this appointment.");

    public static Error NoResourcesProvided => Error.Validation(
        nameof(Appointment.Resources),
        "No resources provided for this appointment.");
}
