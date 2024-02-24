using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.TaskCategories.Queries.GetAllTaskCategories;
using TaskManager.Infrastructure.DAL.EF.Context;
using TaskManager.Shared;

namespace TaskManager.Infrastructure.EF.TaskCategories.Queries.GetAllTaskCategoriesHandler;

public sealed class GetAllTaskCategoriesHandler : IRequestHandler<GetAllTaskCategories, GetAllTaskCategoriesResponse>
{
    private readonly EFContext _context;

    public GetAllTaskCategoriesHandler(EFContext context)
    {
        _context = context;
    }
    
    public async Task<GetAllTaskCategoriesResponse> Handle(GetAllTaskCategories request, CancellationToken cancellationToken)
    {
        var taskCategories = await _context.TaskCategories.AsNoTracking()
            .OrderByDescending(x => x.Name.Equals(Globals.DefaultCategoryName))
            .ThenBy(x => x.Name)
            .Select(x => x.AsTaskCategoryDto())
            .ToListAsync(cancellationToken);

        return new GetAllTaskCategoriesResponse(taskCategories);
    }
}