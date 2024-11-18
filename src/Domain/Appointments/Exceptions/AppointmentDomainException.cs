using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Appointments.Exceptions;

public class AppointmentDomainException : DomainException
{
    public AppointmentDomainException(string message)
        : base(message)
    {
    }

    public AppointmentDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
