using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return "Hello world";
    }
}
