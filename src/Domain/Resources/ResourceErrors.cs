using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Resources;

public static class ResourceErrors
{
    public static readonly Error ErrorName = Error.Validation("name", "Message of error");
}
