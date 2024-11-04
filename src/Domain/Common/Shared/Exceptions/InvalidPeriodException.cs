using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.Shared.Exceptions;

public class InvalidPeriodException : DomainException
{
    public InvalidPeriodException(string message)
        : base(message)
    {
    }
}
