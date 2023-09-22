using FluentValidation;
using TaskManager.Core.Identity.DTOs;

namespace TaskManager.Application.Identity.Commands.SignIn;

public sealed class SignInValidator : AbstractValidator<SignIn>
{
    public SignInValidator()
    {
        RuleFor(x => x.EmailOrUserName).NotEmpty().WithMessage("User name or email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}