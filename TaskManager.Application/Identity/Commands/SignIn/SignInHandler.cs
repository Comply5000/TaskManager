using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;

namespace TaskManager.Application.Identity.Commands.SignIn;

public sealed class SignInHandler : IRequestHandler<SignIn, JsonWebToken>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public SignInHandler(SignInManager<User> signInManager, UserManager<User> userManager, ITokenService tokenService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    public async Task<JsonWebToken> Handle(SignIn request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                    .Where(x => x.UserName == request.EmailOrUserName || x.Email == request.EmailOrUserName)
                    .FirstOrDefaultAsync(cancellationToken) 
                   ?? throw new InvalidCredentialsException();
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if (!result.Succeeded)
             throw new SignInException(result);

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        var jwt = _tokenService.GenerateAccessToken(user.Id, roles, claims);

        jwt.Email = user.Email;

        return jwt;
    }
}