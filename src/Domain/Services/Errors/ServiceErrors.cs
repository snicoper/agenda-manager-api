﻿using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Services.Errors;

public static class ServiceErrors
{
    public static Error NotFound => Error.NotFound("A service with the specified identifier was not found.");

    public static Error NameAlreadyExists => Error.Validation(
        nameof(Service.Name),
        "A service with the same name already exists.");

    public static Error HasAssociatedAppointments => Error.Conflict(
        "The service has associated appointments and it is not possible to delete it");
}
