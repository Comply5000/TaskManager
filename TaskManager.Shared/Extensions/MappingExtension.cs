using TaskManager.Shared.Models;

namespace TaskManager.Shared.Extensions;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
    {
        return PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }

    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, PaginationRequest req)
    {
        return PaginatedList<TDestination>.CreateAsync(queryable, req.PageNumber, req.PageSize);
    }
}