using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxAPI.Models;

namespace ThreadboxAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly ThreadboxContext _context;
        private readonly IMapper _mapper;

        public SectionsController(ThreadboxContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<SectionDto>>> Get()
        {
            var sections = await _context.Sections.ToListAsync();
            return _mapper.Map<List<SectionDto>>(sections);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Create(SectionDto sectionDto)
        {
            var section = _mapper.Map<Section>(sectionDto);
            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> Update(SectionDto sectionDto)
        {
            var section = await _context.Sections.FindAsync(sectionDto.Id);
            if (section == null)
            {
                return NotFound();
            }
            section = _mapper.Map<Section>(sectionDto);
            return Ok();

        }
    }
}