using Microsoft.EntityFrameworkCore;
using ThreadboxAPI.Models;

namespace ThreadboxAPI
{
    public static class Extensions
    {
        public static async Task<Page<T>> ToPagedListAsync<T>(this IQueryable<T> query, PagingParamsDto pagingParams)
        {
            var totalItems = query.Count();
            var pageItems = await query
                .Skip((pagingParams.CurrentPage - 1) * pagingParams.PageSize)
                .Take(pagingParams.PageSize)
                .ToListAsync();

            return new Page<T>(pageItems, pagingParams, totalItems);
        }
    }
}