using FluentValidation;

namespace TaskManager.Application.Emails.Events.ResetPasswordEmail;

public sealed class ResetPasswordEmailValidator : AbstractValidator<ResetPasswordEmail>
{
    public ResetPasswordEmailValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
    }
}