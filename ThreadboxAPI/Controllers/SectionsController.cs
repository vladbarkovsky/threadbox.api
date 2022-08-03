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

        [HttpPost("[action]")]
        public async Task<ActionResult<PagedList<SectionDto>>> Get(PagingParams pagingParams)
        {
            var sections = await _context.Sections.ToPagedListAsync(pagingParams);
            return _mapper.Map<PagedList<SectionDto>>(sections);
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
            var section = _mapper.Map<Section>(sectionDto);
            _context.Sections.Update(section);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var section = await _context.Sections.FindAsync(id);
            if (section == null)
            {
                return NotFound();
            }
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}