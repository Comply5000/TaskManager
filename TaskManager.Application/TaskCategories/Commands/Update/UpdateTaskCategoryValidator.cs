using FluentValidation;

namespace TaskManager.Application.TaskCategories.Commands.Update;

public sealed class UpdateTaskCategoryValidator : AbstractValidator<UpdateTaskCategory>
{
    public UpdateTaskCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}