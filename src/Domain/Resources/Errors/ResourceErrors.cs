using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Resources.Errors;

public static class ResourceErrors
{
    public static Error NotFound => Error.NotFound(
        code: "ResourceErrors.NotFound",
        description: "Resource not found.");

    public static Error NameAlreadyExists => Error.Validation(
        code: nameof(Resource.Name),
        description: "Resource name already exists.");

    public static Error DescriptionAlreadyExists => Error.Validation(
        code: nameof(Resource.Description),
        description: "Resource description already exists.");

    public static Error ResourceNotAvailable => Error.Conflict(
        code: "ResourceErrors.ResourceNotAvailable",
        description: "Resource is not available.");
}
