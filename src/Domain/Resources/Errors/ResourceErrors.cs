using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Resources.Errors;

public static class ResourceErrors
{
    public static Error NotFound => Error.NotFound("Resource not found.");

    public static Error NameAlreadyExists => Error.Validation(nameof(Resource.Name), "Resource name already exists.");

    public static Error DescriptionAlreadyExists => Error.Validation(
        nameof(Resource.Description),
        "Resource description already exists.");

    public static Error ResourceNotAvailable => Error.Conflict("Resource is not available.");
}
