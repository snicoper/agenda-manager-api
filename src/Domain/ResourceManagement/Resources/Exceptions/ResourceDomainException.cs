using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.ResourceManagement.Resources.Exceptions;

public class ResourceDomainException : DomainException
{
    public ResourceDomainException(string message)
        : base(message)
    {
    }

    public ResourceDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
