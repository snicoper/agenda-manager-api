using AgendaManager.Application.Common.Http;
using AgendaManager.Application.ResourceTypes.Queries.GetResourceTypesPaginated;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.ResourceTypes;

[Route("api/v{version:apiVersion}/resource-types")]
public class ResourceTypesController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of resource types.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetResourceTypesPaginatedQueryResponse>>>>
        GetResourceTypesPaginated([FromQuery] RequestData requestData)
    {
        var query = new GetResourceTypesPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }
}
