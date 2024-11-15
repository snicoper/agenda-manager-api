using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Services.Exceptions;

public class ServiceDomainException : DomainException
{
    public ServiceDomainException(string message)
        : base(message)
    {
    }

    public ServiceDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
