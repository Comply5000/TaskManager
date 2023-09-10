using System.Security.Claims;
using TaskManager.Core.Identity.DTOs;

namespace TaskManager.Application.Shared.Common.Identity;

public interface ITokenService
{
    JsonWebToken GenerateAccessToken(Guid userId, ICollection<string> roles, ICollection<Claim> claims);
}