using FluentValidation;

namespace TaskManager.Application.Identity.Commands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePassword>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current password is required");
        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required");
        RuleFor(x => x.NewPasswordConfirmed).NotEmpty().WithMessage("Confirmed new password is required");
        RuleFor(x => x.NewPasswordConfirmed).Equal(x => x.NewPassword).WithMessage("Confirmed password must by the same");
    }
}