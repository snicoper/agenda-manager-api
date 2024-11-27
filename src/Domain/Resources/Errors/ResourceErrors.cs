using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Resources.Errors;

public static class ResourceErrors
{
    public static Error NotFound => Error.NotFound(
        code: "ResourceErrors.NotFound",
        description: "The resource was not found.");

    public static Error NameAlreadyExists => Error.Validation(
        code: nameof(Resource.Name),
        description: "A resource with the specified name already exists.");

    public static Error DescriptionAlreadyExists => Error.Validation(
        code: nameof(Resource.Description),
        description: "A resource with the specified description already exists.");

    public static Error ResourceNotAvailable => Error.Conflict(
        code: "ResourceErrors.ResourceNotAvailable",
        description: "The resource is not available.");
}
