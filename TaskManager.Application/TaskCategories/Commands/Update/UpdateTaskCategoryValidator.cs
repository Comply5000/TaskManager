using FluentValidation;

namespace TaskManager.Application.TaskCategories.Commands.Update;

public sealed class UpdateTaskCategoryValidator : AbstractValidator<UpdateTaskCategory>
{
    public UpdateTaskCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(30).WithMessage("Name max length is 30 characters");

        RuleFor(x => x.Description)
            .MaximumLength(50).WithMessage("Description max lenght is 50 characters");
        
        RuleFor(x => x.PageUrl)
            .MaximumLength(300).WithMessage("Page URL max lenght is 300 characters");
    }
}