using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxAPI.Models;

namespace ThreadboxAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionController : ControllerBase
    {
        private readonly ThreadboxContext _context;

        public SectionController(ThreadboxContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<Section>>> Get()
        {
            var sections = await _context.Sections.ToListAsync();
            if (sections == null)
            {
                return NotFound();
            }
            return sections;
        }
    }
}