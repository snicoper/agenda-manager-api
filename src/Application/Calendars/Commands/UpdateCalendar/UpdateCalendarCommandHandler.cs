using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendar;

internal class UpdateCalendarCommandHandler(
    CalendarManager calendarManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCalendarCommand>
{
    public async Task<Result> Handle(UpdateCalendarCommand request, CancellationToken cancellationToken)
    {
        // 1. Update the calendar and check has update the calendar.
        var updateResult = await calendarManager.UpdateCalendarAsync(
            CalendarId.From(request.CalendarId),
            request.Name,
            request.Description,
            cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        // 2. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
