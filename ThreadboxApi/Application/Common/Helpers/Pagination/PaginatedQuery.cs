using FluentValidation;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public class PaginatedQuery
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}