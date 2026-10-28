namespace ThreadboxApi.Application.Common.Pagination
{
    public abstract class PaginatedQuery
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
    }
}