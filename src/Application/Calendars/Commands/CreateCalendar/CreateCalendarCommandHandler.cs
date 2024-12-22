using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Calendars.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Application.Calendars.Commands.CreateCalendar;

internal class CreateCalendarCommandHandler(CalendarManager calendarManager, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCalendarCommand, CreateCalendarCommandResponse>
{
    public async Task<Result<CreateCalendarCommandResponse>> Handle(
        CreateCalendarCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Create a new calendar.
        var calendarId = CalendarId.Create();
        var ianaTimeZone = IanaTimeZone.FromIana(request.IanaTimeZone);

        var createResult = await calendarManager.CreateCalendarAsync(
            calendarId,
            ianaTimeZone,
            request.Name,
            request.Description,
            cancellationToken);

        if (createResult.IsFailure)
        {
            return createResult.MapToValue<CreateCalendarCommandResponse>();
        }

        // 2. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Return the response.
        var response = new CreateCalendarCommandResponse(createResult.Value!.Id.Value);

        return Result.Create(response);
    }
}
