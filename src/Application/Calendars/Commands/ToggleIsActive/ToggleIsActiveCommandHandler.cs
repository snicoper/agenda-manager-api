using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Commands.ToggleIsActive;

internal class ToggleIsActiveCommandHandler(ICalendarRepository calendarRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ToggleIsActiveCommand>
{
    public async Task<Result> Handle(ToggleIsActiveCommand request, CancellationToken cancellationToken)
    {
        // Get the calendar by id and check if exists.
        var calendar = await calendarRepository.GetByIdAsync(CalendarId.From(request.CalendarId), cancellationToken);
        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // Toggle the IsActive property.
        if (calendar.IsActive)
        {
            calendar.Deactivate();
        }
        else
        {
            calendar.Activate();
        }

        // Save the changes.
        calendarRepository.Update(calendar);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
