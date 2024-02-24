using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskManager.Core.Identity.Services;

namespace TaskManager.Infrastructure.DAL.Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => GetClaimAsGuid(ClaimTypes.NameIdentifier, _httpContextAccessor);
    public string Email 
        => _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    
    private static Guid GetClaimAsGuid(string claimType, IHttpContextAccessor httpContextAccessor)
    {
        var claimAsString = httpContextAccessor?.HttpContext?.User.FindFirstValue(claimType);

        if (string.IsNullOrWhiteSpace(claimAsString))
            return Guid.Empty;

        var parseResultSuccessful = Guid.TryParse(claimAsString, out var claimId);

        if (!parseResultSuccessful || claimId == Guid.Empty)
            return Guid.Empty;

        return claimId;
    }
}