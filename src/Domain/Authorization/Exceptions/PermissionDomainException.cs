using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Authorization.Exceptions;

public class PermissionDomainException(string message) : DomainException(message)
{
}
