using AgendaManager.Application.Accounts.Commands.RecoveryPassword;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Accounts;

[Route("api/v{version:apiVersion}/accounts")]
public class AccountsController : ApiControllerBase
{
    /// <summary>
    /// Send a recovery password email to the user.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("recovery-password")]
    public async Task<ActionResult<Result>> RecoverPassword(RecoveryPasswordCommand command)
    {
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
