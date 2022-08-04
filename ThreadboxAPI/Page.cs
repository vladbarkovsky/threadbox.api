using ThreadboxAPI.Models;

namespace ThreadboxAPI
{
    public class Page<T>
    {
        public List<T> PageItems { get; private set; } = null!;
        public PagingParamsDto PagingParamsDto { get; private set; } = null!;
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
        public bool HasPreviousPage => PagingParamsDto.CurrentPage > 1;
        public bool HasNextPage => PagingParamsDto.CurrentPage < TotalPages;

        // IMPORTANT: Automapper needs parameterless constructor - don't use in code.
        private Page() { }

        public Page(List<T> pageItems, PagingParamsDto pagingParams, int totalItems)
        {
            PageItems = pageItems;
            PagingParamsDto = pagingParams;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(pageItems.Count / (double)PagingParamsDto.PageSize);
        }
    }
}