using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class RoleDomainException(string message) : DomainException(message)
{
}
