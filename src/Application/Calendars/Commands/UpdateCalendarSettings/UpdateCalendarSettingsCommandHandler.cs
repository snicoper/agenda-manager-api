using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendarSettings;

internal class UpdateCalendarSettingsCommandHandler(
    CalendarSettingsManager calendarSettingsManager,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateCalendarSettingsCommand>
{
    public async Task<Result> Handle(UpdateCalendarSettingsCommand request, CancellationToken cancellationToken)
    {
        // Validate the command and change values.
        var calendarUpdateResult = await calendarSettingsManager.UpdateCalendarSettings(
            calendarId: CalendarId.From(request.CalendarId),
            timeZone: IanaTimeZone.FromIana(request.TimeZone),
            appointmentConfirmation: request.AppointmentConfirmationRequirement,
            appointmentOverlapping: request.AppointmentOverlapping,
            holidayConflict: request.HolidayConflict,
            resourceSchedulesAvailability: request.ResourceScheduleValidation);

        if (calendarUpdateResult.IsFailure)
        {
            return calendarUpdateResult;
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
