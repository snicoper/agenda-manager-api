using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Policies;

public class AppointmentConfirmationStrategyPolicy : IAppointmentConfirmationStrategyPolicy
{
    public Result<AppointmentStatus> DetermineInitialStatus(Calendar calendar)
    {
        var defaultStatus =
            calendar.Settings.AppointmentConfirmationRequirement is AppointmentConfirmationRequirementStrategy.Require
                ? AppointmentStatus.Pending
                : AppointmentStatus.Accepted;

        return Result.Success(defaultStatus);
    }
}
