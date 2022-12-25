using Mochieve3.API.Dtos;

namespace ThreadboxApi.Tools
{
	public static class Pagination
	{
		public static PaginatedListDto<T> ToPaginatedListDto<T>(this IEnumerable<T> list, PaginationParamsDto paginationParamsDto)
		{
			var pageIndex = paginationParamsDto.PageIndex;
			var pageSize = paginationParamsDto.PageSize;

			var items = list
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new PaginatedListDto<T>(items, pageIndex, list.Count(), pageSize);
		}
	}
}