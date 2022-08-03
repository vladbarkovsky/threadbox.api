using Microsoft.EntityFrameworkCore;

namespace ThreadboxAPI
{
    public static class Extensions
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, PagingParams pagingParams)
        {
            var totalItems = query.Count();
            var items = await query
                .Skip((pagingParams.CurrentPage - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();

            return new PagedList<T>(items, pagingParams, totalItems);
        }
    }
}