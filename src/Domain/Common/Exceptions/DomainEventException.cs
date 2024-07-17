namespace AgendaManager.Domain.Common.Exceptions;

public class DomainEventException : Exception
{
    public DomainEventException()
    {
    }

    public DomainEventException(string message)
        : base(message)
    {
    }

    public DomainEventException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DomainEventException(string eventName, object key)
        : base($"Exception in \"{eventName}\" ({key}) .")
    {
    }
}
