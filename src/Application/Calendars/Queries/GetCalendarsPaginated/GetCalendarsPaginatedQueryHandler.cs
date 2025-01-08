using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarsPaginated;

internal class GetCalendarsPaginatedQueryHandler(ICalendarRepository calendarRepository)
    : IQueryHandler<GetCalendarsPaginatedQuery, ResponseData<GetCalendarsPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetCalendarsPaginatedQueryResponse>>> Handle(
        GetCalendarsPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        // Get the calendars queryable from the repository.
        var calendars = calendarRepository.GetQueryable()
            .AsQueryable();

        // Create the response data.
        var responseData = await ResponseData<GetCalendarsPaginatedQueryResponse>.CreateAsync(
            source: calendars,
            projection: c => new GetCalendarsPaginatedQueryResponse(
                CalendarId: c.Id.Value,
                Name: c.Name,
                Description: c.Description,
                IsActive: c.IsActive),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
