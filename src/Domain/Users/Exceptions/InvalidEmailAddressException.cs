using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class InvalidEmailAddressException() : DomainException("Invalid email provided.")
{
}
