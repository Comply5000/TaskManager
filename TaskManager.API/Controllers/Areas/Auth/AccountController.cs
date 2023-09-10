using System.Net;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Shared.Common.DTO.Identity;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Services;
using static Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;

namespace TaskManager.API.Controllers.Areas.Auth;

[Route("api/account")]
public class AccountController : BaseController
{
    private readonly IIdentityService _identityService;

    public AccountController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp(SignUpDTO dto, CancellationToken cancellationToken)
    {
        await _identityService.SignUp(dto, cancellationToken);
        return Created(string.Empty, null);
    }
    
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> SignUp(SignInDTO dto, CancellationToken cancellationToken)
    {
        var jwt = await _identityService.SignIn(dto, cancellationToken);
        return Ok(jwt);
    }
    
    [HttpPost("sign-out")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> SignOut(CancellationToken cancellationToken)
    {
        await _identityService.SignOut();
        return Ok();
    }
}