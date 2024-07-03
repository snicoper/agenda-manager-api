using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.Exceptions;

public sealed class InvalidEmailException(string email) : DomainBaseException($"Email {email} is invalid.")
{
}
