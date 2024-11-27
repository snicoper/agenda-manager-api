﻿using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.ResourceTypes.Errors;

public static class ResourceTypeErrors
{
    public static Error NameAlreadyExists => Error.Validation(
        code: nameof(ResourceType.Name),
        description: "The name of the resource type already exists.");

    public static Error DescriptionExists => Error.Validation(
        code: nameof(ResourceType.Description),
        description: "The description of the resource type already exists.");

    public static Error ResourceTypeNotFound => Error.NotFound(
        code: "ResourceTypeErrors.ResourceTypeNotFound",
        description: "Resource type not found.");
}
