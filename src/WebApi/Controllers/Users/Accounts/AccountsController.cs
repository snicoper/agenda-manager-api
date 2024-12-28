using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Users.Accounts.Commands.ConfirmAccount;
using AgendaManager.Application.Users.Accounts.Commands.ConfirmEmail;
using AgendaManager.Application.Users.Accounts.Commands.CreateAccount;
using AgendaManager.Application.Users.Accounts.Commands.RequestPasswordReset;
using AgendaManager.Application.Users.Accounts.Commands.ResentEmailConfirmation;
using AgendaManager.Application.Users.Accounts.Commands.ResetPassword;
using AgendaManager.Application.Users.Accounts.Commands.ToggleIsActive;
using AgendaManager.Application.Users.Accounts.Commands.UpdateAccount;
using AgendaManager.Application.Users.Accounts.Commands.VerifyEmail;
using AgendaManager.Application.Users.Accounts.Queries.GetAccountDetails;
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
    /// Get account details by user id.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<Result<GetAccountDetailsQueryResponse>>> GetAccountDetails(Guid userId)
    {
        var query = new GetAccountDetailsQuery(userId);
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
            Roles: request.Roles);

        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Request a password reset.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("request-password-reset")]
    public async Task<ActionResult<Result>> RequestPasswordReset(RequestPasswordResetRequest resetRequest)
    {
        var command = new RequestPasswordResetCommand(Email: resetRequest.Email);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Resend the email confirmation.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("resend-email-confirmation")]
    public async Task<ActionResult<Result>> ResentEmailConfirmation(ResentEmailConfirmationRequest request)
    {
        var command = new ResentEmailConfirmationCommand(request.Email);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Confirm the user account.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("confirm-account")]
    public async Task<ActionResult<Result>> ConfirmAccount(ConfirmAccountRequest request)
    {
        var command = new ConfirmAccountCommand(request.Token, request.NewPassword, request.ConfirmNewPassword);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Reset the user password.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPut("reset-password")]
    public async Task<ActionResult<Result>> ResetPassword(ResetPasswordRequest request)
    {
        var command = new ResetPasswordCommand(
            Token: request.Token,
            NewPassword: request.NewPassword,
            ConfirmNewPassword: request.ConfirmNewPassword);

        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Verify the email.
    /// </summary>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("verify-email")]
    public async Task<ActionResult<Result>> VerifyEmail(VerifyEmailRequest request)
    {
        var command = new VerifyEmailCommand(request.Token);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Update the account.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId:guid}")]
    public async Task<ActionResult<Result>> UpdateAccount(Guid userId, UpdateAccountRequest request)
    {
        var command = new UpdateAccountCommand(
            UserId: userId,
            FirstName: request.FirstName,
            LastName: request.LastName,
            Phone: new UpdateAccountCommand.PhoneCommand(request.Phone?.Number, request.Phone?.CountryCode),
            Address: new UpdateAccountCommand.AddressCommand(
                Street: request.Address?.Street,
                City: request.Address?.City,
                Country: request.Address?.Country,
                State: request.Address?.State,
                PostalCode: request.Address?.PostalCode),
            IdentityDocument: new UpdateAccountCommand.IdentityDocumentCommand(
                Number: request.IdentityDocument?.Number,
                CountryCode: request.IdentityDocument?.CountryCode,
                Type: request.IdentityDocument?.Type));

        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Toggle the user is active.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId:guid}/toggle-is-active")]
    public async Task<ActionResult<Result>> ToggleIsActive(Guid userId)
    {
        var command = new ToggleIsActiveCommand(userId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }

    /// <summary>
    /// Confirm the email if email is not confirmed.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{userId:guid}/confirm-email")]
    public async Task<ActionResult<Result>> ConfirmEmail(Guid userId)
    {
        var command = new ConfirmEmailCommand(userId);
        var result = await Sender.Send(command);

        return result.ToActionResult();
    }
}
