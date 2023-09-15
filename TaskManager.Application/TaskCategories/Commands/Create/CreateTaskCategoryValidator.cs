using FluentValidation;

namespace TaskManager.Application.TaskCategories.Commands.Create;

public sealed class CreateTaskCategoryValidator : AbstractValidator<CreateTaskCategory>
{
    public CreateTaskCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name max length is 30 characters");

        RuleFor(x => x.Description)
            .MaximumLength(50).WithMessage("Description max lenght is 50 characters");
    }
}