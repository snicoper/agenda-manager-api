using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Appointments.Policies;

public class AppointmentConfirmationStrategyPolicy : IAppointmentConfirmationStrategyPolicy
{
    public Result<AppointmentStatus> DetermineInitialStatus(List<CalendarConfiguration> configurations)
    {
        var creationStrategy = configurations.SingleOrDefault(
            cc => cc.Category == CalendarConfigurationKeys.Appointments.ConfirmationStrategy);

        if (creationStrategy is null)
        {
            return AppointmentErrors.MissingCreationStrategy;
        }

        var defaultStatus = creationStrategy.SelectedKey
            is CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation
            ? AppointmentStatus.Pending
            : AppointmentStatus.Accepted;

        return Result.Success(defaultStatus);
    }
}
