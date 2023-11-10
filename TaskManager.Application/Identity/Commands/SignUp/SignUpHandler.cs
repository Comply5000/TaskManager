using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Emails.Events.ConfirmAccountEmail;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Enums;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.Identity.Static;
using TaskManager.Core.Shared.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Core.TaskCategories.Repositories;
using TaskManager.Shared;
using TaskManager.Shared.Exceptions;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.Identity.Commands.SignUp;

public sealed class SignUpHandler : IRequestHandler<SignUp>
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;

    public SignUpHandler(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }
    
    public async Task Handle(SignUp request, CancellationToken cancellationToken)
    {
        var user = await _identityService.SignUp(request.Email, request.UserName, request.Password, cancellationToken);
        var token = await _identityService.GenerateEmailConfirmationTokenAsync(user, cancellationToken);
        
        await _mediator.Publish(new ConfirmAccountEmail(user.Email!, token, user.Id), cancellationToken);
    }
}