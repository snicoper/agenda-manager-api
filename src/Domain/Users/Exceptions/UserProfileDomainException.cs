using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class UserProfileDomainException(string message) : DomainException(message)
{
}
