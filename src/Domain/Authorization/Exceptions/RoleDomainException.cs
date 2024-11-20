using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Authorization.Exceptions;

public class RoleDomainException(string message) : DomainException(message)
{
}
