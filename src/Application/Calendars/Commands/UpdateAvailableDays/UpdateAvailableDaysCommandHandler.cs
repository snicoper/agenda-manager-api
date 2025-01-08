using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Commands.UpdateAvailableDays;

internal class UpdateAvailableDaysCommandHandler(CalendarManager calendarManager, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAvailableDaysCommand>
{
    public async Task<Result> Handle(UpdateAvailableDaysCommand request, CancellationToken cancellationToken)
    {
        // Update the Available days in the calendar.
        var updateResult = await calendarManager.UpdateAvailableDaysAsync(
            CalendarId.From(request.CalendarId),
            request.AvailableDays,
            cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
