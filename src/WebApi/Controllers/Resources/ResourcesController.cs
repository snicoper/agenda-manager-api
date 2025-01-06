using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Resources.Queries.GetResourcesPaginated;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Resources;

[Route("api/v{version:apiVersion}/resources")]
public class ResourcesController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of resources.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetResourcesPaginatedQueryResponse>>>> GetResourcesPaginated(
        [FromQuery] RequestData requestData)
    {
        var query = new GetResourcesPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }
}
