using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Commands.UpdateAvailableDays;

internal class UpdateAvailableDaysCommandHandler(ICalendarRepository calendarRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAvailableDaysCommand>
{
    public async Task<Result> Handle(UpdateAvailableDaysCommand request, CancellationToken cancellationToken)
    {
        // 1. Get the calendar by id and check if it exists.
        var calendar = await calendarRepository.GetByIdAsync(CalendarId.From(request.CalendarId), cancellationToken);
        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // 2. Update the available days.
        calendar.UpdateAvailableDays(request.AvailableDays);

        // 3. Save changes.
        calendarRepository.Update(calendar);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
