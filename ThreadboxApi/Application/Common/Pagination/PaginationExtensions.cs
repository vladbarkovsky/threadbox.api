using Microsoft.EntityFrameworkCore;

namespace ThreadboxApi.Application.Common.Pagination
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, PaginatedQuery paginatedQuery, CancellationToken cancellationToken = default)
        {
            int pageIndex = paginatedQuery.PageIndex;
            int pageSize = paginatedQuery.PageSize;

            List<T> items = await query
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