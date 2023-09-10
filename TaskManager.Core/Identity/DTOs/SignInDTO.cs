using FluentValidation;

namespace TaskManager.Core.Identity.DTOs;

public sealed record SignInDTO(string? EmailOrUserName, string? Password);

public sealed class SignInDTOValidator : AbstractValidator<SignInDTO>
{
    public SignInDTOValidator()
    {
        RuleFor(x => x.EmailOrUserName).NotEmpty().WithMessage("User name or email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}