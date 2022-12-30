using FluentValidation;

namespace ThreadboxApi.Dtos
{
	public class PaginatedListDto<T>
	{
		public List<T> PageItems { get; } = null!;
		public int PageIndex { get; }

		/// <summary>
		/// Total count of pages. Based on <see cref="PaginationParamsDto"/>.
		/// </summary>
		public int TotalPages { get; }

		/// <summary>
		/// Total count of items from all pages.
		/// </summary>
		public int TotalCount { get; }

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;

		public PaginatedListDto(List<T> pageItems, int pageIndex, int totalCount, int pageSize)
		{
			PageItems = pageItems;
			PageIndex = pageIndex;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			TotalCount = totalCount;
		}
	}

	public class PaginationParamsDto
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }

		public class Validator : AbstractValidator<PaginationParamsDto>
		{
			public Validator()
			{
				RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
				RuleFor(x => x.PageSize).GreaterThan(0);
			}
		}
	}
}