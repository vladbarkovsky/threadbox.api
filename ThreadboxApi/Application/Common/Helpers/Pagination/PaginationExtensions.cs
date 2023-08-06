using Microsoft.EntityFrameworkCore;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, PaginatedQuery paginatedQuery, CancellationToken cancellationToken = default)
        {
            var pageIndex = paginatedQuery.PageIndex;
            var pageSize = paginatedQuery.PageSize;

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<T>(
                pageItems: items,
                pageIndex: pageIndex,
                totalCount: await query.CountAsync(cancellationToken),
                pageSize: pageSize);
        }
    }
}