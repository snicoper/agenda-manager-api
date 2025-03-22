using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.Messaging.Exceptions;

public class OutboxMessageDomainException : DomainException
{
    public OutboxMessageDomainException(string message)
        : base(message)
    {
    }

    public OutboxMessageDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
