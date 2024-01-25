using AutoMapper;

namespace ThreadboxApi.Application.Common.Pagination
{
    public class PaginatedResultMappingProfile : Profile
    {
        public PaginatedResultMappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));
        }
    }
}