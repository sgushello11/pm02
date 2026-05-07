using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtruderProgramsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public ExtruderProgramsController(AgroControlDbContext context)
        {
            _context = context;
        }

        // GET: api/extruderprograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExtruderProgram>>> GetPrograms()
        {
            return await _context.ExtruderPrograms.ToListAsync();
        }

        // GET: api/extruderprograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExtruderProgram>> GetProgram(int id)
        {
            var program = await _context.ExtruderPrograms.FindAsync(id);
            if (program == null)
                return NotFound(new { message = $"Программа с id {id} не найдена" });
            return program;
        }

        // GET: api/extruderprograms/5/zones
        [HttpGet("{id}/zones")]
        public async Task<ActionResult<IEnumerable<ExtruderProgramZone>>> GetProgramZones(int id)
        {
            var zones = await _context.ExtruderProgramZones
                .Where(z => z.ProgramId == id)
                .OrderBy(z => z.ZoneNumber)
                .ToListAsync();
            return zones;
        }

        // POST: api/extruderprograms
        [HttpPost]
        public async Task<ActionResult<ExtruderProgram>> CreateProgram(ExtruderProgram program)
        {
            program.CreatedAt = DateTime.Now;
            _context.ExtruderPrograms.Add(program);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
        }

        // POST: api/extruderprograms/5/zones
        [HttpPost("{id}/zones")]
        public async Task<ActionResult<ExtruderProgramZone>> AddZone(int id, ExtruderProgramZone zone)
        {
            zone.ProgramId = id;
            _context.ExtruderProgramZones.Add(zone);
            await _context.SaveChangesAsync();
            return Ok(zone);
        }

        // PUT: api/extruderprograms/5/activate
        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateProgram(int id)
        {
            var program = await _context.ExtruderPrograms.FindAsync(id);
            if (program == null)
                return NotFound(new { message = $"Программа с id {id} не найдена" });

            program.Status = "active";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Программа активирована", program });
        }
    }
}