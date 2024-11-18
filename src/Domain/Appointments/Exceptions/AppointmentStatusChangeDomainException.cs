using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Appointments.Exceptions;

public class AppointmentStatusChangeDomainException : DomainException
{
    public AppointmentStatusChangeDomainException(string message)
        : base(message)
    {
    }

    public AppointmentStatusChangeDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
