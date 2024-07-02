using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/home")]
public class HomeController : ApiControllerBase
{
    /// <summary>
    /// Retrieves a result containing a greeting message.
    /// </summary>
    /// <returns>An Result object with a greeting message.</returns>
    [HttpGet]
    public ActionResult<Result<string>> Get()
    {
        var result = Result.Create("Hello world");

        return HandleResult(result);
    }
}
