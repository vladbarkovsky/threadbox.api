namespace ThreadboxApi.Application.Common.Pagination
{
    public class PaginatedResult<T>
    {
        public List<T> PageItems { get; init; } = new();
        public int PageIndex { get; init; }

        /// <summary>
        /// Total count of pages. Based on <see cref="PaginatedQuery.PageSize"/>.
        /// </summary>
        public int TotalPages { get; init; }

        /// <summary>
        /// Total count of items from all pages.
        /// </summary>
        public int TotalCount { get; init; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages - 1;

        /// <summary>
        /// Empty constructor for Automapper
        /// </summary>
        public PaginatedResult() { }

        public PaginatedResult(List<T> pageItems, int pageIndex, int totalCount, int pageSize)
        {
            PageItems = pageItems;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            TotalCount = totalCount;
        }
    }
}