using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.TaskCategories.Queries.CheckIfCategoryWithNameExists;
using TaskManager.Infrastructure.DAL.EF.Context;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace TaskManager.Infrastructure.EF.TaskCategories.Queries.CheckIfCategoryWithNameExistsHandler;

public sealed class CheckIfCategoryWithNameExistsHandler : IRequestHandler<CheckIfCategoryWithNameExists, bool>
{
    private readonly EFContext _context;

    public CheckIfCategoryWithNameExistsHandler(EFContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(CheckIfCategoryWithNameExists request, CancellationToken cancellationToken)
    {
        var query = _context.TaskCategories.AsNoTracking();

        if (request.Id is not null)
            query = query.Where(x => x.Id != request.Id);

        return await query.AnyAsync(x => EFCore.Functions.ILike(x.Name, request.Name), cancellationToken);
    }
}