using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Resources.Errors;

public static class ResourceScheduleErrors
{
    public static Error NotFound =>
        Error.NotFound("The resource schedule with the specified identifier was not found.");
}
