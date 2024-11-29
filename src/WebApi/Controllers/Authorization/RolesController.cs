using AgendaManager.Application.Authorization.Queries.GetRolesPaginated;
using AgendaManager.Application.Common.Http;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Authorization;

[Route("api/v{version:apiVersion}/roles")]
public class RolesController : ApiControllerBase
{
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetRolesPaginatedQueryResponse>>>> GetRolesPaginated(
        [FromQuery] RequestData request)
    {
        var result = await Sender.Send(new GetRolesPaginatedQuery(request));

        return result.ToActionResult();
    }
}
