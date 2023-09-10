using TaskManager.Application.Shared.Common.DTO.Identity;
using TaskManager.Core.Identity.DTOs;

namespace TaskManager.Core.Identity.Services;

public interface IIdentityService
{
    Task SignUp(SignUpDTO dto, CancellationToken cancellationToken);
    Task<JsonWebToken> SignIn(SignInDTO dto, CancellationToken cancellationToken);
    Task SignOut();
}