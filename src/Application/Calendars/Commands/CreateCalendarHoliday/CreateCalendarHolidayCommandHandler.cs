using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Application.Calendars.Commands.CreateCalendarHoliday;

internal class CreateCalendarHolidayCommandHandler(
    CalendarHolidayManager calendarHolidayManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCalendarHolidayCommand, CreateCalendarHolidayCommandResponse>
{
    public async Task<Result<CreateCalendarHolidayCommandResponse>> Handle(
        CreateCalendarHolidayCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Create holiday and check is valid.
        var result = await calendarHolidayManager.CreateHolidayAsync(
            CalendarId.From(request.CalendarId),
            Period.From(request.Start, request.End),
            request.Name,
            cancellationToken);

        if (result.IsFailure)
        {
            return result.MapTo<CreateCalendarHolidayCommandResponse>();
        }

        // 2. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Return response.
        var response = new CreateCalendarHolidayCommandResponse(result.Value?.Id.Value ?? Guid.Empty);

        return Result.Create(response);
    }
}
