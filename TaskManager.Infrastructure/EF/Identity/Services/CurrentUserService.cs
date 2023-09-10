using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskManager.Core.Identity.Services;
using TaskManager.Infrastructure.EF.Context;

namespace TaskManager.Infrastructure.EF.Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _userId;
    private readonly ClaimsPrincipal _user;
    private readonly EFContext _context;
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, EFContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        _user = _httpContextAccessor?.HttpContext?.User;
        _context = context;
    }
    
    public Guid UserId => new(_userId);

    public string Email 
        => _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
}