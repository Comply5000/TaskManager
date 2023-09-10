using FluentValidation;

namespace TaskManager.Application.TaskCategories.Commands.Create;

public sealed class CreateTaskCategoryValidator : AbstractValidator<CreateTaskCategory>
{
    public CreateTaskCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}