using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendars;

internal class GetCalendarsQueryHandler(ICalendarRepository calendarRepository)
    : IQueryHandler<GetCalendarsQuery, List<GetCalendarsQueryResponse>>
{
    public async Task<Result<List<GetCalendarsQueryResponse>>> Handle(
        GetCalendarsQuery request,
        CancellationToken cancellationToken)
    {
        // Get all calendars.
        var calendars = await calendarRepository.GetCalendarsAsync(cancellationToken);

        // Map to response.
        var response = calendars.Select(c => new GetCalendarsQueryResponse(c.Id.Value, c.Name)).ToList();

        return Result.Success(response);
    }
}
