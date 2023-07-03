using AutoMapper;
using ThreadboxApi.Application.Common.Helpers.Pagination;
using ThreadboxApi.Application.Common.Interfaces;

namespace ThreadboxApi.Application.Services
{
    public class PaginationService : IScopedService
    {
        private readonly IMapper _mapper;

        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PaginatedResult<TDestination> MapPaginatedResult<TDestination>(PaginatedResult<object> source)
        {
            var destination = Activator.CreateInstance<PaginatedResult<TDestination>>();

            destination.PageItems = _mapper.Map<List<TDestination>>(source.PageItems);
            destination.PageIndex = source.PageIndex;
            destination.TotalPages = source.TotalPages;
            destination.TotalCount = source.TotalCount;

            return destination;
        }
    }
}