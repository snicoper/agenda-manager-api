using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class UserRoleDomainException : DomainException
{
    public UserRoleDomainException(string message)
        : base(message)
    {
    }

    public UserRoleDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
