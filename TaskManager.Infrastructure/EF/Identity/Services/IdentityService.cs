using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Enums;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.Identity.Static;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Exceptions;

namespace TaskManager.Infrastructure.EF.Identity.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly EFContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;

    public IdentityService(EFContext context, UserManager<User> userManager, RoleManager<Role> roleManager, ITokenService tokenService, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }
    
    public async Task SignUp(SignUpDTO dto, CancellationToken cancellationToken)
    {
        var userEmailIsNotUnique = await _context.Users.AnyAsync(x => x.Email == dto.Email, cancellationToken);

        if (userEmailIsNotUnique)
            throw new UserWithThatEmailExistsException();
        
        var userNameIsNotUnique = await _context.Users.AnyAsync(x => x.UserName == dto.UserName, cancellationToken);

        if (userNameIsNotUnique)
            throw new UserWithThatNameExistsException();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            UserStatus = UserStatus.Active
        };

        var createUser = await _userManager.CreateAsync(user, dto.Password);
        
        if (!createUser.Succeeded)
            throw new CreateUserException(createUser.Errors);

        var addRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
        if (!addRoleResult.Succeeded)
            throw new AddToRoleException();

        var addEmailClaimResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email!));
        if (!addEmailClaimResult.Succeeded)
            throw new AddClaimException();
        
        var addNameClaimResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        if (!addNameClaimResult.Succeeded)
            throw new AddClaimException();
        
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<JsonWebToken> SignIn(SignInDTO dto, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking()
                       .Where(x => x.UserName == dto.EmailOrUserName || x.Email == dto.EmailOrUserName)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new InvalidCredentialsException();
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if(!result.Succeeded)
            throw new InvalidCredentialsException();

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        var jwt = _tokenService.GenerateAccessToken(user.Id, roles, claims);

        jwt.Email = user.Email;

        return jwt;
    }

    public async Task SignOut() => await _signInManager.SignOutAsync();
}