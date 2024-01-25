namespace ThreadboxApi.Application.Common.Pagination
{
    public class PaginatedResult<T>
    {
        public List<T> PageItems { get; set; } = new();
        public int PageIndex { get; set; }

        /// <summary>
        /// Total count of pages. Based on <see cref="PaginatedQuery"/>.
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
}