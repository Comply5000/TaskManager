using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.Create;

public sealed class CreateTaskValidator : AbstractValidator<CreateTask>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required");
    }
}