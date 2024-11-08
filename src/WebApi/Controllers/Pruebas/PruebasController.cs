using AgendaManager.Application.Pruebas;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Pruebas;

[Route("api/v{version:apiVersion}/pruebas")]
public class PruebasController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await Sender.Send(new PruebaRequest());

        return Ok(result);
    }
}
