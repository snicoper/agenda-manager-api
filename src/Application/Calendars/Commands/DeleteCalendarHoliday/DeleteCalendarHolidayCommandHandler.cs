using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Commands.DeleteCalendarHoliday;

internal class DeleteCalendarHolidayCommandHandler(ICalendarRepository calendarRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCalendarHolidayCommand>
{
    public async Task<Result> Handle(DeleteCalendarHolidayCommand request, CancellationToken cancellationToken)
    {
        // Get the calendar with the holidays and check if it exists.
        var calendar = await calendarRepository.GetByIdWithHolidaysAsync(
            CalendarId.From(request.CalendarId),
            cancellationToken);

        if (calendar == null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // Get the holiday and check if it exists.
        var holiday = calendar.Holidays.FirstOrDefault(x => x.Id == CalendarHolidayId.From(request.CalendarHolidayId));
        if (holiday == null)
        {
            return CalendarHolidayErrors.CalendarHolidayNotFound;
        }

        // Remove the holiday and save the changes.
        calendar.RemoveHoliday(holiday);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
