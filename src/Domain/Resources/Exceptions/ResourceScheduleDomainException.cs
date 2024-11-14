using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Resources.Exceptions;

public class ResourceScheduleDomainException : DomainException
{
    public ResourceScheduleDomainException(string message)
        : base(message)
    {
    }

    public ResourceScheduleDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
