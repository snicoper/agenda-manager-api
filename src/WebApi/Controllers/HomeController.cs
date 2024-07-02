using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/home")]
public class HomeController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<Result<string>> Get()
    {
        var result = Result.Create("Hello world");

        return HandleResult(result);
    }
}
