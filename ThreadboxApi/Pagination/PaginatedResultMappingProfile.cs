using AutoMapper;

namespace ThreadboxApi.Pagination
{
    public class PaginatedResultMappingProfile : Profile
    {
        public PaginatedResultMappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));
        }
    }
}