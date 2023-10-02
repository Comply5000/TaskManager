using System.Net;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Attributes;
using TaskManager.Application.Emails.Events.ConfirmAccountEmail;
using TaskManager.Application.Emails.Events.ResetPasswordEmail;
using TaskManager.Application.Identity.Commands.ChangePassword;
using TaskManager.Application.Identity.Commands.ConfirmAccount;
using TaskManager.Application.Identity.Commands.ResetPassword;
using TaskManager.Application.Identity.Commands.SignIn;
using TaskManager.Application.Identity.Commands.SignUp;
using TaskManager.Application.Identity.Queries.MyAccountData;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.Identity.Static;
using TaskManager.Shared.Responses;
using static Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;

namespace TaskManager.API.Controllers.Areas.Auth;

[Route($"{Endpoints.BaseUrl}/account")]
public class AccountController : BaseController
{
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp(SignUp command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Created(string.Empty, null);
    }
    
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> SignUp(SignIn command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("confirm-account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmAccount(ConfirmAccount command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("send-reset-password-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendResetPasswordEmail(ResetPasswordEmail command, CancellationToken cancellationToken)
    {
        await Mediator.Publish(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPassword command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpPut("change-password")]
    [ApiAuthorize(Roles = UserRoles.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(ChangePassword command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [HttpGet("my-account-data")]
    [ApiAuthorize(Roles = UserRoles.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MyAccountDataResponse>> MyAccountData(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new MyAccountData(), cancellationToken);
        return OkOrNotFound(result);
    }
}