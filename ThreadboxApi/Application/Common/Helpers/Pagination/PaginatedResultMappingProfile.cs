using AutoMapper;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public class PaginatedResultMappingProfile : Profile
    {
        public PaginatedResultMappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));
        }
    }
}