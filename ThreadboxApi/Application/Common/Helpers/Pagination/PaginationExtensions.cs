using Microsoft.EntityFrameworkCore;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, PaginatedQuery paginatedQuery)
        {
            var pageIndex = paginatedQuery.PageIndex;
            var pageSize = paginatedQuery.PageSize;

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>(items, pageIndex, items.Count, pageSize);
        }
    }
}