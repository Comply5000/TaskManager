using MediatR;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Services;

namespace TaskManager.Application.Identity.Commands.RefreshToken;

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, JsonWebToken>
{
    private readonly IIdentityService _identityService;

    public RefreshTokenHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<JsonWebToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.RefreshTokenAsync(request.Token, cancellationToken);
    }
}