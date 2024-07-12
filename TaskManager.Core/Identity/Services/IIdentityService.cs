using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Entities;

namespace TaskManager.Core.Identity.Services;

public interface IIdentityService
{
    Task<User> SignUp(string email, string userName, string password, CancellationToken cancellationToken);
    Task<JsonWebToken> SignIn(string emailOrUserName, string password, CancellationToken cancellationToken);
    Task SignOut(CancellationToken cancellationToken);
    Task<string> GenerateEmailConfirmationTokenAsync(User user, CancellationToken cancellationToken);
    Task ResetPassword(Guid userId,string token, string password, CancellationToken cancellationToken);
    Task ConfirmAccount(Guid userId, string token, CancellationToken cancellationToken);
    Task ChangePassword(string currentPassword, string newPassword, CancellationToken cancellationToken);
    Task<ResetPasswordTokenDto> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken);
    Task<JsonWebToken> RefreshTokenAsync(string? refreshToken, CancellationToken cancellationToken);
    Task<JsonWebToken> SignInGoogle(CancellationToken cancellationToken);
}