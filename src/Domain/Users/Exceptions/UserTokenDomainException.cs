using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Users.Exceptions;

public class UserTokenDomainException : DomainException
{
    public UserTokenDomainException(string message)
        : base(message)
    {
    }

    public UserTokenDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
