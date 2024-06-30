using AgendaManager.WebApi.Constants;
using AgendaManager.WebApi.Infrastructure;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/home")]
public class HomeController : ApiControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<string> Get()
    {
        return "Hello world 123";
    }
}
