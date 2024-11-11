using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.AuditRecords.Exceptions;

public class AuditRecordDomainException : DomainException
{
    public AuditRecordDomainException(string message)
        : base(message)
    {
    }

    public AuditRecordDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
