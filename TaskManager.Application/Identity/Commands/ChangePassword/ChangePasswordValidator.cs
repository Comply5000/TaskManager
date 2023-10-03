using FluentValidation;

namespace TaskManager.Application.Identity.Commands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePassword>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required");
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
        RuleFor(x => x.NewConfirmedPassword).NotEmpty().WithMessage("Confirmed new password is required");
        RuleFor(x => x.NewConfirmedPassword).Equal(x => x.NewPassword).WithMessage("Confirmed password must by the same");
    }
}