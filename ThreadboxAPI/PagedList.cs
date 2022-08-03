namespace ThreadboxAPI
{
    public class PagedList<T> : List<T>
    {
        public PagingParams PagingParams { get; private set; } = null!;
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
        public bool HasPreviousPage => PagingParams.CurrentPage > 1;
        public bool HasNextPage => PagingParams.CurrentPage < TotalPages;

        // Automapper needs parameterless constructor
        public PagedList() { }

        public PagedList(List<T> items, PagingParams pagingParams, int totalItems)
        {
            PagingParams = pagingParams;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(Count / (double)PagingParams.PageSize);

            AddRange(items);
        }
    }
}
