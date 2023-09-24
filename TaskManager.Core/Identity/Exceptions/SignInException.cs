using Microsoft.AspNetCore.Identity;
using TaskManager.Shared.Abstractions.Exceptions;
using static Microsoft.AspNetCore.Identity.SignInResult;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class SignInException : TaskManagerException
{

    public SignInException(SignInResult result) : base(GetMessageForSignInResult(result)) {}


    private static string GetMessageForSignInResult(SignInResult result)
    {
        if (result == LockedOut)
            return LockOutError;
        if (result == NotAllowed)
            return NotAllowedError;

        return InvalidCredentialsError;

    }

    private const string LockOutError = "Your account has been blocked.";
    private const string NotAllowedError = "Unable to log in to unauthorized account.";
    private const string InvalidCredentialsError = "Invalid credentials.";
}