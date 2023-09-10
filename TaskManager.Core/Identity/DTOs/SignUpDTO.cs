using FluentValidation;

namespace TaskManager.Core.Identity.DTOs;

public sealed record SignUpDTO(string? UserName, string? Email, string? Password, string? ConfirmedPassword);


public sealed class SignUpDTOValidator : AbstractValidator<SignUpDTO>
{
    public SignUpDTOValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email address");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.ConfirmedPassword).NotEmpty().WithMessage("Confirmed password is required");
        RuleFor(x => x.ConfirmedPassword).Equal(x => x.Password).WithMessage("Confirmed password must by the same");
    }
}