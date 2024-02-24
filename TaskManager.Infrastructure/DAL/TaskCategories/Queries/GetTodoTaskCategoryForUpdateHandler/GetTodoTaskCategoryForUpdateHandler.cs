using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.TaskCategories.Queries.GetTaskCategoryForUpdate;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.EF.TaskCategories.Queries.GetTodoTaskCategoryForUpdateHandler;

public sealed class GetTodoTaskCategoryForUpdateHandler : IRequestHandler<GetTaskCategoryForUpdate, GetTaskCategoryForUpdateResponse?>
{
    private readonly EFContext _context;

    public GetTodoTaskCategoryForUpdateHandler(EFContext context)
    {
        _context = context;
    }
    
    public async Task<GetTaskCategoryForUpdateResponse?> Handle(GetTaskCategoryForUpdate request, CancellationToken cancellationToken)
    {
        var todoTaskCategory = await _context.TaskCategories.AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new GetTaskCategoryForUpdateResponse(x.AsTaskCategoryDto()))
            .FirstOrDefaultAsync(cancellationToken);

        return todoTaskCategory;
    }
}