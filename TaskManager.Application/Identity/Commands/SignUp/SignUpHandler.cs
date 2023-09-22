using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Emails.Events.ConfirmAccountEmail;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Enums;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Static;
using TaskManager.Core.Shared.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Core.TaskCategories.Repositories;
using TaskManager.Shared;
using TaskManager.Shared.Exceptions;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.Identity.Commands.SignUp;

public sealed class SignUpHandler : IRequestHandler<SignUp, Unit>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IDateService _dateService;
    private readonly ITaskCategoriesRepository _taskCategoriesRepository;
    private readonly IMediator _mediator;

    public SignUpHandler(SignInManager<User> signInManager, UserManager<User> userManager, ITokenService tokenService, IDateService dateService, ITaskCategoriesRepository taskCategoriesRepository, IMediator mediator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _dateService = dateService;
        _taskCategoriesRepository = taskCategoriesRepository;
        _mediator = mediator;
    }
    
    public async Task<Unit> Handle(SignUp request, CancellationToken cancellationToken)
    {
        var userEmailIsNotUnique = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (userEmailIsNotUnique)
            throw new UserWithThatEmailExistsException();
        
        var userNameIsNotUnique = await _userManager.Users.AnyAsync(x => x.UserName == request.UserName, cancellationToken);

        if (userNameIsNotUnique)
            throw new UserWithThatNameExistsException();
        
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            UserStatus = UserStatus.Unauthorized
        };
        
        var createUser = await _userManager.CreateAsync(user, request.Password!);
        
        if (!createUser.Succeeded)
            throw new CreateUserException(createUser.Errors);

        #region Create default category
        var defaultCategory = TaskCategory.Create(Globals.DefaultCategoryName, null, null);
        defaultCategory.CreatedById = user.Id;
        defaultCategory.CreatedAt = _dateService.CurrentDate();
        await _taskCategoriesRepository.AddAsync(defaultCategory, cancellationToken);
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
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _mediator.Publish(new ConfirmAccountEmail(user.Email!, token, user.Id), cancellationToken);

        return new Unit();

    }
}