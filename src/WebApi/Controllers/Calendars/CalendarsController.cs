using AgendaManager.Application.Calendars.Queries.GetCalendarsPaginated;
using AgendaManager.Application.Common.Http;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Calendars;

[Route("api/v{version:apiVersion}/calendars")]
public class CalendarsController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of calendars.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetCalendarsPaginatedQueryResponse>>>> GetCalendarsPaginated(
        [FromQuery] RequestData requestData)
    {
        var query = new GetCalendarsPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }
}
