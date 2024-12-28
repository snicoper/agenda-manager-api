using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.ResourceManagement.Resources.Errors;

public static class ResourceScheduleErrors
{
    public static Error NotFound => Error.NotFound(
        code: "ResourceScheduleErrors.NotFound",
        description: "The resource schedule was not found..");
}
