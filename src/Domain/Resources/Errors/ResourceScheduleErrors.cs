using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Resources.Errors;

public static class ResourceScheduleErrors
{
    public static Error NotFound => Error.NotFound(
        code: "ResourceScheduleErrors.NotFound",
        description: "The resource schedule was not found..");
}
