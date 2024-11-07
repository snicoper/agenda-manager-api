using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class UserDomainException(string message) : DomainException(message)
{
}
