using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Dtos;

namespace ThreadboxApi.Tools
{
	public class PaginatedResult<T>
	{
		public List<T> PageItems { get; set; } = null!;
		public int PageIndex { get; set; }

		/// <summary>
		/// Total count of pages. Based on <see cref="PaginationParamsDto"/>.
		/// </summary>
		public int TotalPages { get; set; }

		/// <summary>
		/// Total count of items from all pages.
		/// </summary>
		public int TotalCount { get; set; }

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages - 1;

		/// <summary>
		/// Empty constructor for Automapper
		/// </summary>
		public PaginatedResult()
		{ }

		public PaginatedResult(List<T> pageItems, int pageIndex, int totalCount, int pageSize)
		{
			PageItems = pageItems;
			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			TotalCount = totalCount;
		}
	}

	public class PaginatedResultMappingProfile : Profile
	{
		public PaginatedResultMappingProfile()
		{
			CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));
		}
	}

	public static class Pagination
	{
		public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(this IQueryable<T> query, PaginationParamsDto paginationParamsDto)
		{
			var pageIndex = paginationParamsDto.PageIndex;
			var pageSize = paginationParamsDto.PageSize;

			var items = await query
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return new PaginatedResult<T>(items, pageIndex, items.Count, pageSize);
		}
	}
}