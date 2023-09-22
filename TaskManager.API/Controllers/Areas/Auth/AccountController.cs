using System.Net;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Emails.Events.ConfirmAccountEmail;
using TaskManager.Application.Identity.Commands.ConfirmAccount;
using TaskManager.Application.Identity.Commands.SignIn;
using TaskManager.Application.Identity.Commands.SignUp;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Services;
using TaskManager.Shared.Responses;
using static Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;

namespace TaskManager.API.Controllers.Areas.Auth;

[Route($"{Endpoints.BaseUrl}/account")]
[AllowAnonymous]
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
}