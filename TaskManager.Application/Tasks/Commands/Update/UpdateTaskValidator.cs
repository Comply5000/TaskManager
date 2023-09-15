using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.Update;

public sealed class UpdateTaskValidator : AbstractValidator<UpdateTask>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(60).WithMessage("Name max length is 60 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(750).WithMessage("Description max length is 750 characters.");
        
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required");
    }
}