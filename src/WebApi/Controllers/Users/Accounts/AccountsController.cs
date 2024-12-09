using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailResent;
using AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailVerify;
using AgendaManager.Application.Users.Accounts.Commands.ConfirmRecoveryPassword;
using AgendaManager.Application.Users.Accounts.Commands.CreateAccount;
using AgendaManager.Application.Users.Accounts.Commands.RecoveryPassword;
using AgendaManager.Application.Users.Accounts.Queries.GetAccountsPaginated;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;
using AgendaManager.WebApi.Infrastructure;
using AgendaManager.WebApi.Infrastructure.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Controllers.Users.Accounts;

[Route("api/v{version:apiVersion}/accounts")]
public class AccountsController : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of accounts.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("paginated")]
    public async Task<ActionResult<Result<ResponseData<GetAccountsPaginatedQueryResponse>>>> GetAccountsPaginated(
        [FromQuery] RequestData requestData)
    {
        var query = new GetAccountsPaginatedQuery(requestData);
        var result = await Sender.Send(query);

        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new account.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<Result<CreateAccountCommandResponse>>> CreateAccount(CreateAccountRequest request)
    {
        var command = new CreateAccountCommand(
            Email: request.Email,
            FirstName: request.FirstName,
            LastName: request.LastName,
            Password: request.Password,
            PasswordConfirmation: request.PasswordConfirmation,
            Roles: request.Roles,
            IsActive: request.IsActive,
            IsEmailConfirmed: request.IsEmailConfirmed,
            IsCollaborator: request.IsCollaborator);

        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Send a recovery password email to the user.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("recovery-password")]
    public async Task<ActionResult<Result>> RecoverPassword(RecoveryPasswordRequest request)
    {
        var command = new RecoveryPasswordCommand(Email: request.Email);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Confirm the recovery password.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("confirm-recovery-password")]
    public async Task<ActionResult<Result>> ConfirmRecoveryPassword(ConfirmRecoveryPasswordRequest request)
    {
        var command = new ConfirmRecoveryPasswordCommand(
            Token: request.Token,
            NewPassword: request.NewPassword,
            ConfirmNewPassword: request.ConfirmNewPassword);

        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Confirm the email.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("confirm-email-resent")]
    public async Task<ActionResult<Result>> ConfirmEmailResent(ConfirmEmailResentRequest request)
    {
        var command = new ConfirmEmailResentCommand(request.Email);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Verify the email.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("confirm-email-verify")]
    public async Task<ActionResult<Result>> ConfirmEmailVerify(ConfirmEmailVerifyRequest request)
    {
        var command = new ConfirmEmailVerifyCommand(request.Token);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
