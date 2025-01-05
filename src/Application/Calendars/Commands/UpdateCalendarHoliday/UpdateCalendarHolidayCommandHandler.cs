using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendarHoliday;

internal class UpdateCalendarHolidayCommandHandler(
    CalendarHolidayManager calendarHolidayManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCalendarHolidayCommand>
{
    public async Task<Result> Handle(UpdateCalendarHolidayCommand request, CancellationToken cancellationToken)
    {
        // Update calendar holiday and check result.
        var result = await calendarHolidayManager.UpdateHolidayAsync(
            CalendarId.From(request.CalendarId),
            CalendarHolidayId.From(request.CalendarHolidayId),
            request.Name,
            Period.From(request.Start, request.End),
            cancellationToken);

        if (result.IsFailure)
        {
            return result;
        }

        // Save changes and return result.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
