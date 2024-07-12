using System.Net;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManager.API.Attributes;
using TaskManager.API.Common.Static;
using TaskManager.Application.Emails.Events.ConfirmAccountEmail;
using TaskManager.Application.Emails.Events.ResetPasswordEmail;
using TaskManager.Application.Identity.Commands.ChangePassword;
using TaskManager.Application.Identity.Commands.ConfirmAccount;
using TaskManager.Application.Identity.Commands.RefreshToken;
using TaskManager.Application.Identity.Commands.ResetPassword;
using TaskManager.Application.Identity.Commands.SignIn;
using TaskManager.Application.Identity.Commands.SignInGoogle;
using TaskManager.Application.Identity.Commands.SignOut;
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
    private readonly SignInManager<Core.Identity.Entities.User> _signInManager;

    public AccountController(SignInManager<Core.Identity.Entities.User> signInManager)
    {
        _signInManager = signInManager;
    }
    
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
        SetRefreshTokenCookie(result.RefreshToken);
        return Ok(result);
    }
    
    [HttpPost("sign-out")]
    [ApiAuthorize(Roles = UserRoles.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> SignOut(SignOut command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        Response.Cookies.Delete(Tokens.RefreshToken);
        return Ok();
    }
    
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[Tokens.RefreshToken];
        var result = await Mediator.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        SetRefreshTokenCookie(result.RefreshToken);
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
    
    [HttpGet("signin-google")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return Challenge(properties, "Google");
    }
    
    [HttpGet("google-response")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JsonWebToken>> GoogleResponse(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SignInGoogleCommand(), cancellationToken);
        var sessionId = Guid.NewGuid().ToString();
            
        HttpContext.Session.SetString(sessionId, JsonConvert.SerializeObject(result));
            
        var redirectUrl = $"{Shared.Globals.ApplicationUrl}/google-response?sessionId={sessionId}";
        return Redirect(redirectUrl);
    }
        
    [HttpGet("get-session-data/{sessionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<JsonWebToken>> GetSessionData(string sessionId)
    {
        if (HttpContext.Session.TryGetValue(sessionId, out var sessionData))
        {
            var json = System.Text.Encoding.UTF8.GetString(sessionData);
            var result = JsonConvert.DeserializeObject<JsonWebToken>(json);
                
            HttpContext.Session.Remove(sessionId);
            SetRefreshTokenCookie(result.RefreshToken);
                
            return Ok(result);
        }
        return Unauthorized();
    }
    
    private void SetRefreshTokenCookie(RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = refreshToken.Expires,
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        Response.Cookies.Append(Tokens.RefreshToken, refreshToken.Token, cookieOptions);
    }
}