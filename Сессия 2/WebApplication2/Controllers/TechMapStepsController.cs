using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechMapStepsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public TechMapStepsController(AgroControlDbContext context)
        {
            _context = context;
        }

        // GET: api/techmapsteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechMapStep>>> GetTechMapSteps()
        {
            return await _context.TechMapSteps.ToListAsync();
        }

        // GET: api/techmapsteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TechMapStep>> GetTechMapStep(int id)
        {
            var step = await _context.TechMapSteps.FindAsync(id);
            if (step == null)
                return NotFound(new { message = $"Шаг тех. карты с id {id} не найден" });
            return step;
        }

        // GET: api/techmapsteps/techmap/1
        [HttpGet("techmap/{techMapId}")]
        public async Task<ActionResult<IEnumerable<TechMapStep>>> GetStepsByTechMap(int techMapId)
        {
            var steps = await _context.TechMapSteps.Where(s => s.TechMapId == techMapId).OrderBy(s => s.StepOrder).ToListAsync();
            return steps;
        }

        // GET: api/techmapsteps/equipment/1
        [HttpGet("equipment/{equipmentId}")]
        public async Task<ActionResult<IEnumerable<TechMapStep>>> GetStepsByEquipment(int equipmentId)
        {
            var steps = await _context.TechMapSteps.Where(s => s.EquipmentId == equipmentId).ToListAsync();
            return steps;
        }

        // POST: api/techmapsteps
        [HttpPost]
        public async Task<ActionResult<TechMapStep>> CreateTechMapStep(TechMapStep step)
        {
            _context.TechMapSteps.Add(step);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTechMapStep), new { id = step.Id }, step);
        }

        // PUT: api/techmapsteps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechMapStep(int id, TechMapStep step)
        {
            if (id != step.Id)
                return BadRequest(new { message = "ID не совпадают" });

            _context.Entry(step).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/techmapsteps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechMapStep(int id)
        {
            var step = await _context.TechMapSteps.FindAsync(id);
            if (step == null)
                return NotFound(new { message = $"Шаг тех. карты с id {id} не найден" });

            _context.TechMapSteps.Remove(step);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Шаг удален", step });
        }
    }
}