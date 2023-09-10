using FluentValidation;

namespace TaskManager.Application.Tasks.Commands.Update;

public sealed class UpdateTaskValidator : AbstractValidator<UpdateTask>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required");
    }
}