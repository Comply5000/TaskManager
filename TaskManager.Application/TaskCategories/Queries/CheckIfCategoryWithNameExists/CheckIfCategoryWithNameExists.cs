using MediatR;

namespace TaskManager.Application.TaskCategories.Queries.CheckIfCategoryWithNameExists;

public sealed record CheckIfCategoryWithNameExists(string Name, Guid? Id = null) : IRequest<bool>;
