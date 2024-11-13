using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.ResourceTypes.Errors;

public static class ResourceTypeErrors
{
    public static Error NameAlreadyExists => Error.Validation(nameof(ResourceType.Name), "Name already exists.");

    public static Error DescriptionExists => Error.Validation(
        nameof(ResourceType.Description),
        "Description already exists.");

    public static Error ResourceTypeNotFound => Error.NotFound("Resource type not found.");
}
