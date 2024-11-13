using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.ResourceTypes.Exceptions;

public class ResourceTypeDomainException : DomainException
{
    public ResourceTypeDomainException(string message)
        : base(message)
    {
    }

    public ResourceTypeDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
