using MediatR;
using TaskManager.Core.Identity.Services;

namespace TaskManager.Application.Identity.Commands.SignOut;

public sealed class SignOutHandler : IRequestHandler<SignOut>
{
    private readonly IIdentityService _identityService;

    public SignOutHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task Handle(SignOut request, CancellationToken cancellationToken)
    {
        await _identityService.SignOut(cancellationToken);
    }
}