using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class PermissionDomainException(string message) : DomainException(message)
{
}
