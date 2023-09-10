using System.Security.Claims;
using TaskManager.Application.Shared.Common.DTO.Identity;

namespace TaskManager.Application.Shared.Common.Identity;

public interface ITokenService
{
    JsonWebToken GenerateAccessToken(Guid userId, ICollection<string> roles, ICollection<Claim> claims);
}