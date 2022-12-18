using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mochieve3.API.Dtos;
using ThreadboxApi.Configuration;
using ThreadboxApi.Dtos;

namespace ThreadboxAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly ThreadboxDbContext _context;
        private readonly IMapper _mapper;

        public BoardsController(IServiceProvider services)
        {
            _context = services.GetRequiredService<ThreadboxDbContext>();
            _mapper = services.GetRequiredService<IMapper>();
        }

        [HttpPost("[action]")]
        public ActionResult<PaginatedListDto<BoardDto>> Get(PaginationParamsDto paginationParamsDto)
        {
            return _mapper.Map<PaginatedListDto<BoardDto>>(_context.Boards);
        }
    }
}