using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Shared.Services;
using TaskManager.Shared.Configurations.Identity;
using JsonWebToken = TaskManager.Core.Identity.DTOs.JsonWebToken;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TaskManager.Infrastructure.EF.Identity.Services;

public class TokenService : ITokenService
{
    private static readonly Dictionary<string, string> EmptyClaims = new();
    private static readonly ICollection<string> EmptyRoles = new List<string>();
    private readonly IDateService _dateService;
    private readonly AuthConfig _authConfig;
    private readonly SigningCredentials _signingCredentials;
    private readonly string _issuer;

    public TokenService(IDateService dateService, AuthConfig authConfig)
    {
        _dateService = dateService;
        _authConfig = authConfig;
        _signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.JwtKey)),
                SecurityAlgorithms.HmacSha256);
        _issuer = authConfig.JwtIssuer;
    }
    
    public JsonWebToken GenerateAccessToken(Guid userId, ICollection<string> roles, ICollection<Claim> claims)
    {
        var now = _dateService.CurrentDate();
        
        var jwtClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
        };
        
        if (roles?.Any() is true) // or check if it is empty - Every collection in Model is initialized 
            foreach (var role in roles)
                jwtClaims.Add(new Claim("role", role));
        
        if (claims?.Any() is true)
        {
            var customClaims = new List<Claim>();
            foreach (var claim in claims)
            {
                customClaims.Add(new Claim(claim.Type, claim.Value));
            }

            jwtClaims.AddRange(customClaims);
        }

        var expires = now.Add(_authConfig.Expires);
        
        var jwt = new JwtSecurityToken(
            _issuer,
            _issuer,
            jwtClaims,
            now,
            expires,
            _signingCredentials);
        
        // Writing Token
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        // Mapping token to more user friendly DTO
        return new JsonWebToken()
        {
            AccessToken = token,
            Expires = new DateTimeOffset(expires).ToUnixTimeSeconds(),
            UserId = userId,
            Roles = roles ?? EmptyRoles,
            Claims = claims?.ToDictionary(a => a.Type, a => a.Value) ?? EmptyClaims
        };
    }
}