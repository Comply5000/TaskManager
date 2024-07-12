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
using TaskManager.Core.Shared.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Infrastructure.DAL.EF.Context;
using TaskManager.Shared;
using TaskManager.Shared.Exceptions;

namespace TaskManager.Infrastructure.DAL.Identity.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IDateService _dateService;
    private readonly EFContext _context;
    private readonly ITokenService _tokenService;
    private readonly ICurrentUserService _currentUserService;

    public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IDateService dateService, EFContext context, ITokenService tokenService, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dateService = dateService;
        _context = context;
        _tokenService = tokenService;
        _currentUserService = currentUserService;
    }
    
    public async Task<User> SignUp(string email, string userName, string password, CancellationToken cancellationToken)
    {
        var userEmailIsNotUnique = await _userManager.Users.AnyAsync(x => x.Email == email, cancellationToken);

        if (userEmailIsNotUnique)
            throw new UserWithThatEmailExistsException();
        
        var userNameIsNotUnique = await _userManager.Users.AnyAsync(x => x.UserName == userName, cancellationToken);

        if (userNameIsNotUnique)
            throw new UserWithThatNameExistsException();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        var user = new User
        {
            UserName = userName,
            Email = email,
            UserStatus = UserStatus.Active
        };
        
        var createUser = await _userManager.CreateAsync(user, password);
        
        if (!createUser.Succeeded)
            throw new CreateUserException(createUser.Errors);

        #region Create default category
        var defaultCategory = TaskCategory.Create(Globals.DefaultCategoryName, null, null);
        defaultCategory.CreatedById = user.Id;
        defaultCategory.CreatedAt = _dateService.CurrentOffsetDate();
        await _context.AddAsync(defaultCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        #endregion

        var addRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
        if (!addRoleResult.Succeeded)
            throw new AddToRoleException();

        var addEmailClaimResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email!));
        if (!addEmailClaimResult.Succeeded)
            throw new AddClaimException();
        
        var addNameClaimResult = await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        if (!addNameClaimResult.Succeeded)
            throw new AddClaimException();

        await transaction.CommitAsync(cancellationToken);
        
        return user;
    }

    public async Task<JsonWebToken> SignIn(string emailOrUserName, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                       .Where(x => x.UserName == emailOrUserName || x.Email == emailOrUserName)
                       .FirstOrDefaultAsync(cancellationToken) 
                   ?? throw new InvalidCredentialsException();
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
        // if (result.IsLockedOut)
        //     throw new UserLockedOutException(user.Id, user.LockoutEnd, "jakieś info");
        if (!result.Succeeded)
            throw new SignInException(result);

        var jwt = await GenerateJwtAsync(user, cancellationToken);

        return jwt;
    }

    public async Task SignOut(CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId, cancellationToken)
                   ?? throw new UserNotFoundException();

        user.RefreshToken = null;
        
        _context.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        await _signInManager.SignOutAsync();
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task ResetPassword(Guid userId,string token, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                   ?? throw new ResetPasswordException();

        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
            throw new ChangePasswordException(result.Errors);
    }

    public async Task ConfirmAccount(Guid userId, string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                   ?? throw new ConfirmAccountException();

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            throw new ConfirmAccountException();
    }

    public async Task ChangePassword(string currentPassword, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(x => x.Id == _currentUserService.UserId)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new UserNotFoundException();
        
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, currentPassword);

        if (isPasswordCorrect is false)
            throw new InvalidPasswordException();
        
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
            throw new ChangePasswordException(result.Errors);
    }

    public async Task<ResetPasswordTokenDto> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.AsNoTracking()
                       .Where(x => x.Email == email)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new UserWithEmailDoesntExistException();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return new ResetPasswordTokenDto
        {
            Token = token,
            UserId = user.Id
        };
    }

    public async Task<JsonWebToken> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken)
                   ?? throw new InvalidRefreshTokenException();
        
        if(user.RefreshTokenExpires <= _dateService.CurrentOffsetDate())
            throw new InvalidRefreshTokenException();

        var jwt = await GenerateJwtAsync(user, cancellationToken);
        
        return jwt;
    }

    public async Task<JsonWebToken> SignInGoogle(CancellationToken cancellationToken)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
            throw new UserNotFoundException();
        
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        
        if (result.Succeeded)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == info.Principal.FindFirst(ClaimTypes.Email).Value, cancellationToken)
                       ?? throw new UserNotFoundException();
            
            return await GenerateJwtAsync(user, cancellationToken);
        }

        var newUser = new User
        {
            Email = info.Principal.FindFirst(ClaimTypes.Email)?.Value,
            UserName = info.Principal.FindFirst(ClaimTypes.Email)?.Value,
            EmailConfirmed = true,
            UserStatus = UserStatus.Active
        };
        
        var createResult = await _userManager.CreateAsync(newUser);
        if (!createResult.Succeeded) 
            throw new CreateUserException(createResult.Errors);
        
        #region Create default category
        var defaultCategory = TaskCategory.Create(Globals.DefaultCategoryName, null, null);
        defaultCategory.CreatedById = newUser.Id;
        defaultCategory.CreatedAt = _dateService.CurrentOffsetDate();
        await _context.AddAsync(defaultCategory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        #endregion
        
        var addRoleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        if (!addRoleResult.Succeeded)
            throw new AddToRoleException();

        var addEmailClaimResult = await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Email, newUser.Email));
        if (!addEmailClaimResult.Succeeded)
            throw new AddClaimException();
        
        var addNameClaimResult = await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()));
        if (!addNameClaimResult.Succeeded)
            throw new AddClaimException();
        
        var addLoginResult = await _userManager.AddLoginAsync(newUser, info);
        if (!addLoginResult.Succeeded)
            throw new CreateUserException(addLoginResult.Errors);
        
        await _signInManager.RefreshSignInAsync(newUser);
        
        return await GenerateJwtAsync(newUser, cancellationToken);
    }

    private async Task<JsonWebToken> GenerateJwtAsync(User user, CancellationToken cancellationToken)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);
            
        var jwt = _tokenService.GenerateAccessToken(user.Id, userRoles, userClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        jwt.Email = user.Email;
        jwt.RefreshToken = refreshToken;
        
        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpires = refreshToken.Expires;
        
        _context.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return jwt;
    }
}