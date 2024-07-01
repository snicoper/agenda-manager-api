using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/home")]
public class HomeController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<Result<string>> Get()
    {
        var result = Result.Create("Error");

        return HandleResult(result);
    }
}
