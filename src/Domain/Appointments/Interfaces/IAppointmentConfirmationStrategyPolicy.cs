using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Interfaces;

public interface IAppointmentConfirmationStrategyPolicy
{
    Result<AppointmentStatus> DetermineInitialStatus(List<CalendarConfiguration> configurations);
}
