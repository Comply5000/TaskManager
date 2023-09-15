using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.Create;

public sealed class CreateTaskValidator : AbstractValidator<CreateTask>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(60).WithMessage("Name max length is 60 characters");
        
        RuleFor(x => x.Description)
            .MaximumLength(750).WithMessage("Description max length is 750 characters");
            
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required");
    }
}