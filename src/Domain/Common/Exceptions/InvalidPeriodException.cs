namespace AgendaManager.Domain.Common.Exceptions;

public class InvalidPeriodException : DomainException
{
    public InvalidPeriodException(string message)
        : base(message)
    {
    }
}
