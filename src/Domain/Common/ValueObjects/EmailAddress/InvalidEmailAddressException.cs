using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects.EmailAddress;

public class InvalidEmailAddressException() : DomainException("Invalid email provided.")
{
}
